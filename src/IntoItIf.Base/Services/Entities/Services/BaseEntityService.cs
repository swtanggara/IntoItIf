namespace IntoItIf.Base.Services.Entities.Services
{
   using System;
   using Domain.Entities;
   using Domain.Options;
   using Interceptors;
   using Repositories;
   using UnitOfWork;

   public abstract class BaseEntityService<T, TInterceptor, TRepository> : IEntityService
      where T : class, IEntity
      where TInterceptor : IInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      protected BaseEntityService() : this(InjecterGetter.GetUow())
      {
      }

      protected BaseEntityService(Option<IUow> uow) : this(uow, Activator.CreateInstance<TInterceptor>())
      {
      }

      protected BaseEntityService(Option<IUow> uow, Option<TInterceptor> interceptor)
      {
         ServiceMapping = new ServiceMapping<T, TInterceptor, TRepository>(uow, interceptor);
      }

      #endregion

      #region Properties

      protected Option<ServiceMapping<T, TInterceptor, TRepository>> ServiceMapping { get; }

      #endregion
   }
}