namespace IntoItIf.MongoDb
{
   using System;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Helpers;
   using Base.Helpers.Lazy;
   using MongoDB.Driver;

   public class MongoExpiringSession
   {
      #region Static Fields

      private static readonly AsyncExpiringLazy<IClientSessionHandle> ExpiringSessionLazy =
         new AsyncExpiringLazy<IClientSessionHandle>(_ => InitSessionAsync(_client, _options, _expiringInSeconds, _ctok));

      private static IMongoClient _client;
      private static double _expiringInSeconds;
      private static CancellationToken _ctok;

      private static ClientSessionOptions _options = new ClientSessionOptions
      {
         DefaultTransactionOptions = new TransactionOptions(
            ReadConcern.Snapshot,
            ReadPreference.Nearest,
            WriteConcern.WMajority)
      };

      #endregion

      #region Public Methods and Operators

      public static IClientSessionHandle Get(IMongoClient client, ClientSessionOptions options, double expiringInSeconds)
      {
         return AsyncHelper.RunSync(() => GetAsync(client, options, expiringInSeconds));
      }

      public static Task<IClientSessionHandle> GetAsync(IMongoClient client, ClientSessionOptions options, double expiringInSeconds, CancellationToken ctok = default)
      {
         _client = client;
         _options = options;
         _expiringInSeconds = expiringInSeconds;
         _ctok = ctok;
         return ExpiringSessionLazy.Value();
      }

      #endregion

      #region Methods

      private static Task<IClientSessionHandle> GetSessionAsync(
         IMongoClient client,
         ClientSessionOptions options,
         CancellationToken ctok = default)
      {
         return client.StartSessionAsync(options, ctok);
      }

      private static async Task<ExpiringLazyMetadata<IClientSessionHandle>> InitSessionAsync(
         IMongoClient client,
         ClientSessionOptions options,
         double expiringInSeconds,
         CancellationToken ctok = default)
      {
         return new ExpiringLazyMetadata<IClientSessionHandle>
         {
            Result = await GetSessionAsync(client, options, ctok),
            ValidUntil = DateTimeOffset.UtcNow.AddSeconds(expiringInSeconds),
         };
      }

      #endregion
   }
}