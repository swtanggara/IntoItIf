#if NETSTANDARD2_0
namespace IntoItIf.Dal.UnitOfWorks
{
   using Microsoft.EntityFrameworkCore.Storage;

   internal sealed class UowCoreDbTransaction
      : IUowDbTransaction<IDbContextTransaction>
   {
      internal UowCoreDbTransaction(IDbContextTransaction transaction)
      {
         Transaction = transaction;
      }

      public IDbContextTransaction Transaction { get; }

      public void Dispose()
      {
         Transaction?.Dispose();
      }

      public void Commit()
      {
         Transaction.Commit();
      }

      public void Rollback()
      {
         Transaction.Rollback();
      }
   }
}
#endif