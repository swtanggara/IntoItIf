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
   using MongoDB.Bson.Serialization.IdGenerators;
   using MongoDB.Driver;

   public abstract class MongoDataContext : IRelationalDataContext
   {
      #region Constants

      private const string ThisIsNotCodeFirstStyleBro =
         "This is not Code-First style coding bro. Please create your collection first on MongoDB database";

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
               ReadPreference.Primary,
               WriteConcern.WMajority)
         };
         ModelBuilder = new MongoModelBuilder();
         BuildModel(ModelBuilder.ReduceOrDefault());
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
                     if (!(definition.Value is StartModelParameter startParam)) continue;
                     foreach (var indexParam in startParam.IndexModelParameters)
                     {
                        if (!indexParam.Options.Unique.GetValueOrDefault()) continue;
                        foreach (var dictProperty in indexParam.Rendered.ToDictionary())
                        {
                           var pi = type.GetProperty(dictProperty.Key);
                           if (pi != null) result.Add(pi);
                        }
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
         foreach (var pi in type.GetProperties())
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

      internal Option<IClientSessionHandle> GetNewMongoSession()
      {
         return GetClient()
            .Map(
               x =>
               {
                  var session = x.StartSession(_sessionOptions);
                  return session;
               });
      }

      internal async Task<Option<IClientSessionHandle>> GetNewMongoSessionAsync(Option<CancellationToken> ctok)
      {
         return await GetClient()
            .Combine(ctok, true, CancellationToken.None)
            .Map(x => (Client: x.Item1, Ctok: x.Item2))
            .MapAsync(
               async x =>
               {
                  var session = await x.Client.StartSessionAsync(_sessionOptions, x.Ctok);
                  return session;
               })
            .ConfigureAwait(false);
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
         BsonSerializer.RegisterIdGenerator(typeof(Guid), GuidGenerator.Instance);
      }

      private void Dispose(bool disposing)
      {
         if (!_disposed && disposing)
         {
            ModelBuilder
               .Execute(
                  x =>
                  {
                     if (x == null) return;
                     x.ModelDefinitions?.Clear();
                     x.ModelDefinitions = null;
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

      internal Option<bool> RegisterChangesWatch<T>(
         Option<ChangeStreamOperationType> operationType,
         Action<ChangeStreamDocument<T>> action)
      {
         return Collection<T>()
            .Combine(operationType)
            .Combine(action.ToOption())
            .Map(x => (Collection: x.Item1.Item1, OpsType: x.Item1.Item2, Action: x.Item2))
            .Execute(
               async x =>
               {
                  var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<T>>().Match(y => y.OperationType == x.OpsType);
                  using (var cursor = await x.Collection.WatchAsync(pipeline))
                  {
                     await cursor.ForEachAsync(x.Action);
                  }
               });
      }
   }
}