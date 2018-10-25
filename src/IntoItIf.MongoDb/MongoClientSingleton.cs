namespace IntoItIf.MongoDb
{
   using System;
   using System.Threading.Tasks;
   using Base.Helpers;
   using Base.Helpers.Lazy;
   using MongoDB.Driver;

   public class MongoClientSingleton
   {
      #region Static Fields

      private static readonly AsyncLazy<MongoClient> MongoClientLazy =
         new AsyncLazy<MongoClient>(_ => InitMongoClientAsync(_connectionString));

      private static readonly AsyncExpiringLazy<MongoClient> ExpiringMongoClientLazy =
         new AsyncExpiringLazy<MongoClient>(_ => InitExpiringMongoClientAsync(_connectionString, _expiringInMinutes));

      private static string _connectionString = string.Empty;

      private static double _expiringInMinutes = 120;

      #endregion

      #region Public Methods and Operators

      public static MongoClient Get(string connectionString)
      {
         return AsyncHelper.RunSync(() => GetAsync(connectionString));
      }

      public static MongoClient Get(string connectionString, double expiringMinutes)
      {
         return AsyncHelper.RunSync(() => GetAsync(connectionString, expiringMinutes));
      }

      public static Task<MongoClient> GetAsync(string connectionString)
      {
         _connectionString = connectionString;
         return MongoClientLazy.Value();
      }

      public static Task<MongoClient> GetAsync(string connectionString, double expiringInMinutes)
      {
         _connectionString = connectionString;
         _expiringInMinutes = expiringInMinutes;
         return ExpiringMongoClientLazy.Value();
      }

      #endregion

      #region Methods

      private static MongoClient GetClient(string connectionString)
      {
         return new MongoClient(connectionString);
      }

      private static Task<ExpiringLazyMetadata<MongoClient>> InitExpiringMongoClientAsync(
         string connectionString,
         double expiringInMinutes)
      {
         var result = new ExpiringLazyMetadata<MongoClient>
         {
            Result = GetClient(connectionString),
            ValidUntil = DateTimeOffset.UtcNow.AddMinutes(expiringInMinutes)
         };
         return Task.FromResult(result);
      }

      private static Task<LazyMetadata<MongoClient>> InitMongoClientAsync(string connectionString)
      {
         var result = new LazyMetadata<MongoClient>
         {
            Result = GetClient(connectionString)
         };
         return Task.FromResult(result);
      }

      #endregion
   }
}