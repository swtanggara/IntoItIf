namespace IntoItIf.Base.UnitOfWork
{
   using System;
   using Domain.Options;
   using Repositories;
   using Services;

   public interface IUow : IDisposable, IInjectable
   {
      #region Public Methods and Operators

      Option<TRepository> GetRepository<TRepository, T>()
         where TRepository : class, IRepository<T>
         where T : class;

      #endregion
   }
}