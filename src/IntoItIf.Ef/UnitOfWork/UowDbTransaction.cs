#if NET471
namespace IntoItIf.Ef.UnitOfWork
{
   using System.Data.Entity;
   using Base.UnitOfWork;

   internal sealed class UowDbTransaction : IUowDbTransaction<DbContextTransaction>
   {
      #region Constructors and Destructors

      internal UowDbTransaction(DbContextTransaction transaction)
      {
         Transaction = transaction;
      }

      #endregion

      #region Public Properties

      public DbContextTransaction Transaction { get; }

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