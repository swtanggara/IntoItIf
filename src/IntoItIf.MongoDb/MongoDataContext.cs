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

      protected MongoDataContext(string connectionString, string dbName, FindOptions findOptions)
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
         BuildModel(ModelBuilder);
      }

      #endregion

      #region Properties

      internal FindOptions FindOptions { get; }
      protected string ConnectionString { get; }
      protected string DbName { get; }
      protected MongoModelBuilder ModelBuilder { get; }

      #endregion

      #region Public Methods and Operators

      public (Expression<Func<T, bool>> Predicate, string[] PropertyNames) BuildAlternateKeyPredicate<T>(T entity)
         where T : class
      {
         var propertyNames = GetAlternateKeyProperties<T>().Select(y => y.Name).ToArray();
         return (entity.BuildEqualPredicateFor(propertyNames), propertyNames);
      }

      public (Expression<Func<T, bool>> Predicate, string[] PropertyNames) BuildPrimaryKeyPredicate<T>(T entity)
         where T : class
      {
         var propertyNames = GetPrimaryKeyProperties<T>().Select(y => y.Name).ToArray();
         return (entity.BuildEqualPredicateFor(propertyNames), propertyNames);
      }

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      public IEnumerable<PropertyInfo> GetAlternateKeyProperties<T>()
         where T : class
      {
         var result = new List<PropertyInfo>();
         var type = typeof(T);
         foreach (var definition in ModelBuilder.ModelDefinitions)
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
      }

      public IEnumerable<PropertyInfo> GetPrimaryKeyProperties<T>()
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

      public IQueryable<T> GetQuery<T>()
         where T : class
      {
         return Collection<T>().AsQueryable();
      }

      public IMongoCollection<T> Collection<T>()
      {
         var type = typeof(T);
         if (!ModelBuilder.ModelDefinitions.ContainsKey(type))
         {
            throw new InvalidOperationException("Please call your Entity<T>() on OnModelCreating() first");
         }

         if (!_existingCollectionNames.Contains(type.Name))
         {
            throw new InvalidOperationException(ThisIsNotCodeFirstStyleBro);
         }

         return GetDatabase().GetCollection<T>(typeof(T).Name);
      }

      #endregion

      #region Methods

      internal IClientSessionHandle GetNewMongoSession()
      {
         return GetClient().StartSession(_sessionOptions);
      }

      internal Task<IClientSessionHandle> GetNewMongoSessionAsync(CancellationToken ctok)
      {
         return GetClient().StartSessionAsync(_sessionOptions, ctok);
      }

      protected abstract void OnModelCreating(MongoModelBuilder modelBuilder);

      private void BuildModel(MongoModelBuilder mongoIndexBuilder)
      {
         OnModelCreating(mongoIndexBuilder);
         var db = GetDatabase();
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
            if (ModelBuilder == null) return;
            ModelBuilder.ModelDefinitions?.Clear();
            ModelBuilder.ModelDefinitions = null;
         }

         _disposed = true;
      }

      private MongoClient GetClient()
      {
         return MongoClientSingleton.Get(ConnectionString);
      }

      private IMongoDatabase GetDatabase()
      {
         return GetClient().GetDatabase(DbName);
      }

      #endregion

      internal async Task RegisterChangesWatchAsync<T>(
         ChangeStreamOperationType operationType,
         Action<ChangeStreamDocument<T>> action)
      {
         var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<T>>().Match(y => y.OperationType == operationType);
         using (var cursor = await Collection<T>().WatchAsync(pipeline))
         {
            await cursor.ForEachAsync(action);
         }
      }
   }
}