namespace IntoItIf.MongoDb
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Reflection;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.DataContexts;
   using Base.Domain.Options;
   using Base.Helpers;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization;
   using MongoDB.Bson.Serialization.Attributes;
   using MongoDB.Driver;

   public abstract class MongoDataContext : IRelationalDataContext
   {
      #region Constants

      private const string ThisIsNotCodeFirstStyleBro =
         "This is not Code-First style coding bro. Please create your collection first on MongoDB database";
      private const double ImplicitSessionExpiringInSeconds = 10;

      #endregion

      #region Fields

      private readonly ClientSessionOptions _sessionOptions;
      private bool _disposed;
      private List<string> _existingCollectionNames;

      #endregion

      #region Constructors and Destructors

      protected MongoDataContext(Option<string> connectionString, Option<string> dbName, Option<FindOptions> findOptions)
      {
         ConnectionString = connectionString;
         DbName = dbName;
         FindOptions = findOptions;
         _sessionOptions = new ClientSessionOptions
         {
            DefaultTransactionOptions = new TransactionOptions(
               ReadConcern.Snapshot,
               ReadPreference.Nearest,
               WriteConcern.WMajority)
         };
         ModelBuilder = new MongoModelBuilder();
         BuildModel(ModelBuilder.ReduceOrDefault());
         ImplicitSession = None.Value;
         ExplicitSession = None.Value;
      }

      #endregion

      #region Properties

      internal Option<IClientSessionHandle> ExplicitSession { get; private set; }
      internal Option<FindOptions> FindOptions { get; }
      internal Option<IClientSessionHandle> ImplicitSession { get; private set; }
      protected Option<string> ConnectionString { get; }
      protected Option<string> DbName { get; }
      protected Option<MongoModelBuilder> ModelBuilder { get; }

      #endregion

      #region Public Methods and Operators

      public Option<(Expression<Func<T, bool>> Predicate, string[] PropertyNames)> BuildAlternateKeyPredicate<T>(T entity)
         where T : class
      {
         return GetAlternateKeyProperties<T>().Map(x => x.Select(y => y.Name).ToArray()).Map(x => (entity.BuildEqualPredicateFor(x), x));
      }

      public Option<(Expression<Func<T, bool>> Predicate, string[] PropertyNames)> BuildPrimaryKeyPredicate<T>(T entity)
         where T : class
      {
         return GetPrimaryKeyProperties<T>().Map(x => x.Select(y => y.Name).ToArray()).Map(x => (entity.BuildEqualPredicateFor(x), x));
      }

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      public Option<IEnumerable<PropertyInfo>> GetAlternateKeyProperties<T>()
         where T : class
      {
         return ModelBuilder.Map(
               x =>
               {
                  var result = new List<PropertyInfo>();
                  var type = typeof(T);
                  foreach (var definition in x.ModelDefinitions)
                  {
                     if (!(definition.Value is CreateIndexModelParameter indexParam)) continue;
                     foreach (var dictProperty in indexParam.Rendered.ToDictionary())
                     {
                        var pi = type.GetProperty(dictProperty.Key, BindingFlags.Public);
                        if (pi != null) result.Add(pi);
                     }
                  }

                  return result;
               })
            .ReduceOrDefault();
      }

      public Option<IEnumerable<PropertyInfo>> GetPrimaryKeyProperties<T>()
         where T : class
      {
         var type = typeof(T);
         foreach (var pi in type.GetProperties(BindingFlags.Public))
         {
            var isId = Attribute.IsDefined(pi, typeof(BsonIdAttribute)) || pi.Name == "Id";
            if (isId) return new List<PropertyInfo> { pi };
         }

         return new List<PropertyInfo>();
      }

      public Option<IQueryable<T>> GetQuery<T>()
         where T : class
      {
         IQueryable<T> result = Collection<T>()
            .Map(x => x.AsQueryable())
            .ReduceOrDefault();
         return result.ToOption();
      }

      public Option<IMongoCollection<T>> Collection<T>()
      {
         return GetDatabase()
            .Combine(ModelBuilder)
            .Map(x => (Database: x.Item1, ModelBuilder: x.Item2))
            .Map(
               x =>
               {
                  var type = typeof(T);
                  if (!x.ModelBuilder.ModelDefinitions.ContainsKey(type))
                  {
                     throw new InvalidOperationException("Please call your Entity<T>() on OnModelCreating() first");
                  }

                  if (!_existingCollectionNames.Contains(type.Name))
                  {
                     throw new InvalidOperationException(ThisIsNotCodeFirstStyleBro);
                  }

                  return x.Database.GetCollection<T>(typeof(T).Name);
               });
      }

      #endregion

      #region Methods

      internal Option<IClientSessionHandle> GetExplicitMongoSession()
      {
         if (!ExplicitSession.IsSome())
         {
            ExplicitSession = GetClient()
               .Map(
                  x =>
                  {
                     var session = x.StartSession(_sessionOptions);
                     session.StartTransaction();
                     return session;
                  });
         }
         else
         {
            ExplicitSession.IfExecute(x => !x.IsInTransaction, x => x.StartTransaction(_sessionOptions.DefaultTransactionOptions));
         }

         return ExplicitSession;
      }

      internal async Task<Option<IClientSessionHandle>> GetExplicitMongoSessionAsync(Option<CancellationToken> ctok)
      {
         if (!ExplicitSession.IsSome())
         {
            ExplicitSession = await GetClient()
               .Combine(ctok, true, CancellationToken.None)
               .Map(x => (Client: x.Item1, Ctok: x.Item2))
               .MapAsync(async x =>
               {
                  var session = await x.Client.StartSessionAsync(_sessionOptions, x.Ctok);
                  session.StartTransaction();
                  return session;
               })
               .ConfigureAwait(false);
         }
         else
         {
            ExplicitSession.IfExecute(x => !x.IsInTransaction, x => x.StartTransaction(_sessionOptions.DefaultTransactionOptions));
         }

         return ExplicitSession;
      }

      internal Option<IClientSessionHandle> GetImplicitMongoSession()
      {
         if (!ImplicitSession.IsSome())
         {
            ImplicitSession = GetClient()
               .Map(
                  x =>
                  {
                     var session = MongoExpiringSession.Get(x, _sessionOptions, ImplicitSessionExpiringInSeconds);
                     if (!session.IsInTransaction) session.StartTransaction();
                     return session;
                  });
         }
         else
         {
            ImplicitSession.IfExecute(x => !x.IsInTransaction, x => x.StartTransaction(_sessionOptions.DefaultTransactionOptions));
         }

         return ImplicitSession;
      }

      internal async Task<Option<IClientSessionHandle>> GetImplicitMongoSessionAsync(Option<CancellationToken> ctok)
      {
         if (!ImplicitSession.IsSome())
         {
            ImplicitSession = await GetClient()
               .Combine(ctok, true, CancellationToken.None)
               .Map(x => (Client: x.Item1, Ctok: x.Item2))
               .MapAsync(
                  async x =>
                  {
                     var session = await MongoExpiringSession.GetAsync(x.Client, _sessionOptions, ImplicitSessionExpiringInSeconds, x.Ctok);
                     if (!session.IsInTransaction) session.StartTransaction();
                     return session;
                  })
               .ConfigureAwait(false);
         }
         else
         {
            ImplicitSession.IfExecute(x => !x.IsInTransaction, x => x.StartTransaction(_sessionOptions.DefaultTransactionOptions));
         }

         return ImplicitSession;
      }

      protected abstract void OnModelCreating(MongoModelBuilder modelBuilder);

      private void BuildModel(MongoModelBuilder mongoIndexBuilder)
      {
         OnModelCreating(mongoIndexBuilder);
         var db = GetDatabase().ReduceOrDefault();
         _existingCollectionNames = db.ListCollectionNames().ToList();
         foreach (var modelDefinition in mongoIndexBuilder.ModelDefinitions)
         {
            var key = modelDefinition.Key;
            if (!_existingCollectionNames.Contains(key.Name))
            {
               throw new InvalidOperationException(ThisIsNotCodeFirstStyleBro);
            }

            if (!(modelDefinition.Value is StartModelParameter param) || param.Type != key) continue;
            var collection = db.GetCollection<BsonDocument>(key.Name);
            foreach (var indexModelParameter in param.IndexModelParameters)
            {
               collection.Indexes.CreateOne(
                  new CreateIndexModel<BsonDocument>(
                     new BsonDocumentIndexKeysDefinition<BsonDocument>(indexModelParameter.Rendered),
                     indexModelParameter.Options));
            }
         }
      }

      private void Dispose(bool disposing)
      {
         if (!_disposed && disposing)
         {
            ModelBuilder
               .Combine(ImplicitSession)
               .Combine(ExplicitSession)
               .Map(x => (IndexBuilder: x.Item1.Item1, ImplicitSession: x.Item1.Item2, ExplicitSession: x.Item2))
               .Execute(
                  x =>
                  {
                     if (x.IndexBuilder != null)
                     {
                        x.IndexBuilder.ModelDefinitions?.Clear();
                        x.IndexBuilder.ModelDefinitions = null;
                     }

                     x.ImplicitSession?.Dispose();
                     x.ExplicitSession?.Dispose();
                  });
         }

         _disposed = true;
      }

      private Option<MongoClient> GetClient()
      {
         return ConnectionString.Map(MongoClientSingleton.Get);
      }

      private Option<IMongoDatabase> GetDatabase()
      {
         return GetClient().Map(x => x.GetDatabase(DbName.ReduceOrDefault()));
      }

      #endregion
   }
}