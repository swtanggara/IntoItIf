namespace IntoItIf.Base.Services.Entities
{
   using Domain.Entities;
   using Domain.Options;
   using Helpers;
   using Interceptors;
   using Repositories;
   using UnitOfWork;

   #region New region

   public class ServiceMapping<T, TInterceptor, TRepository>
      where T : class, IEntity
      where TInterceptor : IInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public ServiceMapping(Option<ISaveUow> uow, Option<TInterceptor> interceptor) : this(
         uow,
         uow.ReduceOrDefault().GetRepository<TRepository, T>(),
         interceptor)
      {
      }

      public ServiceMapping(
         Option<ISaveUow> uow,
         Option<TRepository> repository,
         Option<TInterceptor> interceptor)
      {
         Uow = uow;
         Repository = repository;
         Interceptor = interceptor;
      }

      #endregion

      #region Properties

      internal Option<TInterceptor> Interceptor { get; }
      internal Option<TRepository> Repository { get; }
      internal Option<ISaveUow> Uow { get; }

      #endregion

      #region Methods

      internal Option<ServiceMapping<T, TInterceptor2, TRepository>> ChangeInterceptor<TInterceptor2>()
         where TInterceptor2 : IInterceptor
      {
         return new ServiceMapping<T, TInterceptor2, TRepository>(
            Uow,
            Repository,
            Interceptor.ReduceOrDefault().As<TInterceptor2>());
      }

      #endregion
   }

   #endregion
}