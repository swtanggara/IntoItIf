namespace IntoItIf.Dsl.Entities.Services
{
   using System;
   using System.Collections.Generic;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;
   using Interceptors;

   public abstract class BaseEntityService<T, TInterceptor> : ValueObject<BaseEntityService<T, TInterceptor>>, IEntityService
      where T : class, IEntity
      where TInterceptor : IInterceptor
   {
      #region Constructors and Destructors

      protected BaseEntityService() : this(DslInjecterGetter.GetBaseUnitOfWork())
      {
      }

      protected BaseEntityService(Option<BaseUnitOfWork> uow) : this(uow, Activator.CreateInstance<TInterceptor>())
      {
      }

      protected BaseEntityService(Option<BaseUnitOfWork> uow, Option<TInterceptor> interceptor)
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