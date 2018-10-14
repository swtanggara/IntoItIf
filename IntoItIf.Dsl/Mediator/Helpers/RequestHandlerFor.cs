namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Entities.Interceptors;
   using Handlers;

   public abstract class RequestHandlerFor<T, TInterceptor, TRequestHandler>
      where TRequestHandler : BaseRequestHandler<T, TInterceptor>
      where T : class, IEntity
      where TInterceptor : IInterceptor
   {
      #region Constructors and Destructors

      internal RequestHandlerFor()
      {
      }

      #endregion

      #region Methods

      protected Option<TRequestHandler> Handler()
      {
         return Activator.CreateInstance<TRequestHandler>();
      }

      #endregion
   }
}