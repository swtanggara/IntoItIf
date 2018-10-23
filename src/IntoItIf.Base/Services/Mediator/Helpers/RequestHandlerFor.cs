namespace IntoItIf.Base.Services.Mediator.Helpers
{
   using System;
   using Domain.Entities;
   using Domain.Options;
   using Entities.Interceptors;
   using Handlers;
   using Repositories;
   using UnitOfWork;

   public abstract class RequestHandlerFor<T, TInterceptor, TRequestHandler, TRepository>
      where TRequestHandler : BaseRequestHandler<T, TInterceptor, TRepository>
      where T : class, IEntity
      where TInterceptor : IInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      protected RequestHandlerFor(Option<IUow> uow)
      {
         Uow = uow;
      }

      #endregion

      #region Properties

      protected Option<IUow> Uow { get; }

      #endregion

      #region Methods

      protected Option<TRequestHandler> Handler()
      {
         return Activator.CreateInstance(typeof(TRequestHandler), Uow) as TRequestHandler;
      }

      #endregion
   }
}