namespace IntoItIf.Ef.UnitOfWork
{
   using Base.Domain.Options;
   using Base.Repositories;
   using Base.UnitOfWork;

   public interface IEfUow : IStringCommandUow, ITransactionUow, ISaveUow
   {
      Option<BaseRelationalRepository<T>> SetOf<T>()
         where T : class;
   }
}