namespace IntoItIf.Base.UnitOfWork
{
   using System;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Options;
   using Repositories;
   using Services;

   public interface IUow : IDisposable, IInjectable
   {
      #region Public Methods and Operators

      Option<TRepository> GetRepository<TRepository, T>()
         where TRepository : class, IRepository<T>
         where T : class;

      Option<int> SaveChanges();
      Task<Option<int>> SaveChangesAsync();
      Task<Option<int>> SaveChangesAsync(Option<CancellationToken> ctok);

      #endregion
   }
}