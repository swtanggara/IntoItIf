namespace IntoItIf.Dal.UnitOfWorks
{
   using System;

   public interface IUowDbTransaction : IDisposable
   {
      #region Public Methods and Operators

      void Commit();
      void Rollback();

      #endregion
   }

   public interface IUowDbTransaction<out TTransaction> : IUowDbTransaction
      where TTransaction : IDisposable
   {
      #region Public Properties

      TTransaction Transaction { get; }

      #endregion
   }
}