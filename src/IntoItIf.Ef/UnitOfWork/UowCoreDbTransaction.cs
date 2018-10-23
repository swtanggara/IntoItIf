#if NETSTANDARD2_0
namespace IntoItIf.Ef.UnitOfWork
{
   using Base.UnitOfWork;
   using Microsoft.EntityFrameworkCore.Storage;

   internal sealed class UowCoreDbTransaction
      : IUowDbTransaction<IDbContextTransaction>
   {
      #region Constructors and Destructors

      internal UowCoreDbTransaction(IDbContextTransaction transaction)
      {
         Transaction = transaction;
      }

      #endregion

      #region Public Properties

      public IDbContextTransaction Transaction { get; }

      #endregion

      #region Public Methods and Operators

      public void Commit()
      {
         Transaction.Commit();
      }

      public void Dispose()
      {
         Transaction?.Dispose();
      }

      public void Rollback()
      {
         Transaction.Rollback();
      }

      #endregion
   }
}
#endif