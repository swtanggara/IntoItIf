namespace IntoItIf.MongoDb
{
   using System.Threading;
   using System.Threading.Tasks;
   using Base.UnitOfWork;
   using MongoDB.Driver;

   public class MongoUowTransaction : IUowDbTransaction<IClientSessionHandle>
   {
      #region Constructors and Destructors

      internal MongoUowTransaction(IClientSessionHandle session)
      {
         Transaction = session;
         if (!Transaction.IsInTransaction) Transaction.StartTransaction();
      }

      #endregion

      #region Public Properties

      public IClientSessionHandle Transaction { get; }

      #endregion

      #region Public Methods and Operators

      public void Commit()
      {
         Transaction.CommitTransaction();
      }

      public Task CommitAsync(CancellationToken ctok = default)
      {
         return Transaction.CommitTransactionAsync(ctok);
      }

      public void Dispose()
      {
         Transaction?.Dispose();
      }

      public void Rollback()
      {
         Transaction.AbortTransaction();
      }

      public Task RollbackAsync(CancellationToken ctok = default)
      {
         return Transaction.AbortTransactionAsync(ctok);
      }

      #endregion
   }
}