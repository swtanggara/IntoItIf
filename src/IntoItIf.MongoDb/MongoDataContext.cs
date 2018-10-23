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
   using MongoDB.Bson.Serialization.Attributes;
   using MongoDB.Driver;

   public abstract class MongoDataContext : IRelationalDataContext
   {
      #region Fields

      private bool _disposed;
      private List<string> _existingCollectionNames;

      private Option<IClientSessionHandle> _session;

      #endregion

      #region Constructors and Destructors

      protected MongoDataContext(Option<string> connectionString, Option<string> dbName, Option<FindOptions> findOptions)
      {
         ConnectionString = connectionString;
         DbName = dbName;
         FindOptions = findOptions;
         ModelBuilder = new MongoModelBuilder();
         BuildModel(ModelBuilder.ReduceOrDefault());
         _session = None.Value;
      }

      #endregion

      #region Properties

      internal Option<FindOptions> FindOptions { get; }
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

      public Option<IClientSessionHandle> GetMongoSession()
      {
         if (!_session.IsSome())
         {
            _session = GetClient().Map(x => x.StartSession());
         }
         else
         {
            _session.IfExecute(x => !x.IsInTransaction, x => x.StartTransaction());
         }

         return _session;
      }

      public async Task<Option<IClientSessionHandle>> GetMongoSessionAsync(Option<CancellationToken> ctok)
      {
         if (!_session.IsSome())
         {
            _session = await GetClient()
               .Combine(ctok, true, CancellationToken.None)
               .Map(x => (Client: x.Item1, Ctok: x.Item2))
               .MapAsync(x => x.Client.StartSessionAsync(cancellationToken: x.Ctok))
               .ConfigureAwait(false);
         }
         else
         {
            _session.IfExecute(
               x => !x.IsInTransaction,
               x => x.StartTransaction(new TransactionOptions(ReadConcern.Snapshot, writeConcern: WriteConcern.WMajority)));
         }

         return _session;
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
         IQueryable<T> result = Set<T>()
            .Map(x => x.AsQueryable())
            .ReduceOrDefault();
         return result.ToOption();
      }

      public Option<IMongoCollection<T>> Set<T>()
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
                     throw new InvalidOperationException(
                        "This is not Code-First style coding bro. Please create your collection first on MongoDB database");
                  }

                  return x.Database.GetCollection<T>(
                     typeof(T).Name,
                     new MongoCollectionSettings
                     {
                        AssignIdOnInsert = true,
                        GuidRepresentation = GuidRepresentation.Standard,
                     });
               });
      }

      #endregion

      #region Methods

      protected abstract void OnModelCreating(MongoModelBuilder modelBuilder);

      private void BuildModel(MongoModelBuilder mongoIndexBuilder)
      {
         OnModelCreating(mongoIndexBuilder);
         var db = GetDatabase().ReduceOrDefault();
         _existingCollectionNames = db.ListCollectionNames().ToList();
         foreach (var indexBuilder in mongoIndexBuilder.ModelDefinitions)
         {
            var key = indexBuilder.Key;
            if (!_existingCollectionNames.Contains(key.Name))
            {
               throw new InvalidOperationException(
                  "This is not Code-First style coding bro. Please create your collection first on MongoDB database");
            }

            if (!(indexBuilder.Value is StartModelParameter param) || param.Type != key) continue;
            var collection = db.GetCollection<BsonDocument>(indexBuilder.Key.Name);
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
            ModelBuilder.Combine(_session)
               .Map(x => (IndexBuilder: x.Item1, Session: x.Item2))
               .Execute(
                  x =>
                  {
                     if (x.IndexBuilder != null)
                     {
                        x.IndexBuilder.ModelDefinitions?.Clear();
                        x.IndexBuilder.ModelDefinitions = null;
                     }

                     x.Session?.Dispose();
                  });
         }

         _disposed = true;
      }

      private Option<MongoClient> GetClient()
      {
         return ConnectionString.Map(x => new MongoClient(x));
      }

      private Option<IMongoDatabase> GetDatabase()
      {
         return GetClient().Map(x => x.GetDatabase(DbName.ReduceOrDefault()));
      }

      #endregion
   }
}