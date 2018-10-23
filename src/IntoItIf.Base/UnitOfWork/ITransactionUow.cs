namespace IntoItIf.Base.UnitOfWork
{
   using DataContexts;

   public interface ITransactionUow : IUow
   {
      #region Public Methods and Operators

      IUowDbTransaction GetDbTransaction<TContext>()
         where TContext : class, IDataContext;

      #endregion
   }
}