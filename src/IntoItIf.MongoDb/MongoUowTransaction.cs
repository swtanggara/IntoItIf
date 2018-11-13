namespace IntoItIf.MongoDb
{
   using System.Threading;
   using System.Threading.Tasks;
   using MongoDB.Driver;

   public class MongoUowTransaction : IMongoUowDbTransaction
   {
      #region Constructors and Destructors

      internal MongoUowTransaction(IClientSessionHandle session)
      {
         Transaction = session;
         if (!Transaction.IsInTransaction) Transaction.StartTransaction();
      }

      #endregion

      #region Public Properties

      public IClientSessionHandle Transaction { get; private set; }

      #endregion

      #region Public Methods and Operators

      public void Commit()
      {
         if (Transaction.IsInTransaction) Transaction.CommitTransaction();
      }

      public async Task CommitAsync(CancellationToken ctok = default)
      {
         if (Transaction.IsInTransaction) await Transaction.CommitTransactionAsync(ctok);
      }

      public void Dispose()
      {
         if (Transaction == null) return;
         if (Transaction.IsInTransaction) Transaction.AbortTransaction();
         Transaction.Dispose();
         Transaction = null;
      }

      public void Rollback()
      {
         if (Transaction.IsInTransaction) Transaction.AbortTransaction();
      }

      public async Task RollbackAsync(CancellationToken ctok = default)
      {
         if (Transaction.IsInTransaction) await Transaction.AbortTransactionAsync(ctok);
      }

      #endregion
   }
}