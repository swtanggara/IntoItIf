namespace IntoItIf.Base.Services.Mediator.Handlers
{
   using System;
   using Domain.Entities;
   using Domain.Options;
   using Entities;
   using Entities.Interceptors;
   using Repositories;
   using UnitOfWork;

   public abstract class BaseRequestHandler<T, TInterceptor, TRepository>
      where T : class, IEntity
      where TInterceptor : IInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      protected BaseRequestHandler(Option<ISaveUow> uow) : this(uow, Activator.CreateInstance<TInterceptor>())
      {
      }

      protected BaseRequestHandler(Option<ISaveUow> uow, Option<TInterceptor> interceptor)
      {
         ServiceMapping = new ServiceMapping<T, TInterceptor, TRepository>(uow, interceptor);
      }

      #endregion

      #region Properties

      protected Option<ServiceMapping<T, TInterceptor, TRepository>> ServiceMapping { get; }

      #endregion
   }
}