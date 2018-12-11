namespace IntoItIf.Base.UnitOfWork
{
   using System;
   using Repositories;
   using Services;

   public interface IUow : IDisposable, IInjectable
   {
      #region Public Methods and Operators

      TRepository GetRepository<TRepository, T>()
         where TRepository : class, IRepository<T>
         where T : class;

      #endregion
   }
}