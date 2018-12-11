namespace IntoItIf.Ef.UnitOfWork
{
   using Base.Repositories;
   using Base.UnitOfWork;

   public interface IEfUow : IStringCommandUow, ITransactionUow, ISaveUow
   {
      BaseRelationalRepository<T> SetOf<T>()
         where T : class;
   }
}