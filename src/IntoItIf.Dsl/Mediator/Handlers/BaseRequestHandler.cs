namespace IntoItIf.Dsl.Mediator.Handlers
{
   using System;
   using System.Collections.Generic;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;
   using Entities;
   using Entities.Interceptors;

   public abstract class BaseRequestHandler<T, TInterceptor> : ValueObject<BaseRequestHandler<T, TInterceptor>>
      where T : class, IEntity
      where TInterceptor : IInterceptor
   {
      #region Constructors and Destructors

      protected BaseRequestHandler() : this(DslInjecterGetter.GetBaseUnitOfWork())
      {
      }

      protected BaseRequestHandler(Option<BaseUnitOfWork> uow) : this(uow, Activator.CreateInstance<TInterceptor>())
      {
      }

      protected BaseRequestHandler(Option<BaseUnitOfWork> uow, Option<TInterceptor> interceptor)
      {
         ServiceMapping = new ServiceMapping<T, TInterceptor>(uow, interceptor);
      }

      #endregion

      #region Properties

      protected Option<ServiceMapping<T, TInterceptor>> ServiceMapping { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return ServiceMapping;
      }

      #endregion
   }
}