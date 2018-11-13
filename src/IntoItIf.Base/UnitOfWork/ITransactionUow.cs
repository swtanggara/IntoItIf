namespace IntoItIf.Base.UnitOfWork
{
   public interface ITransactionUow : IUow
   {
      #region Public Methods and Operators

      IUowDbTransaction GetDbTransaction();

      #endregion
   }
}