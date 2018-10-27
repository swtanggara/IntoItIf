namespace IntoItIf.MongoDb
{
   using System;
   using Base.Helpers.Lazy;
   using MongoDB.Driver;

   public class MongoClientSingleton
   {
      #region Static Fields

      private static readonly Lazy<MongoClient> MongoClientLazy =
         new Lazy<MongoClient>(() => InitMongoClient(_connectionString));

      private static readonly ExpiringLazy<MongoClient> ExpiringMongoClientLazy =
         new ExpiringLazy<MongoClient>(_ => InitExpiringMongoClient(_connectionString, _expiringInMinutes));

      private static string _connectionString = string.Empty;

      private static double _expiringInMinutes = 120;

      #endregion

      #region Public Methods and Operators

      public static MongoClient Get(string connectionString)
      {
         _connectionString = connectionString;
         return MongoClientLazy.Value;
      }

      public static MongoClient Get(string connectionString, double expiringInMinutes)
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

      private static ExpiringLazyMetadata<MongoClient> InitExpiringMongoClient(
         string connectionString,
         double expiringInMinutes)
      {
         return new ExpiringLazyMetadata<MongoClient>
         {
            Result = GetClient(connectionString),
            ValidUntil = DateTimeOffset.UtcNow.AddMinutes(expiringInMinutes)
         };
      }

      private static MongoClient InitMongoClient(string connectionString)
      {
         return new MongoClient(connectionString);
      }

      #endregion
   }
}