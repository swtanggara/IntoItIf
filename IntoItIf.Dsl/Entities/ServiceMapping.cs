namespace IntoItIf.Dsl.Entities
{
   using System.Collections.Generic;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;
   using Helpers;
   using Interceptors;

   public class ServiceMapping<T, TInterceptor> : ValueObject<ServiceMapping<T, TInterceptor>>
      where T : class, IEntity
      where TInterceptor : IInterceptor
   {
      #region Constructors and Destructors

      public ServiceMapping(Option<BaseUnitOfWork> uow, Option<TInterceptor> interceptor) : this(
         uow,
         uow.ReduceOrDefault().GetRepository<T>(),
         interceptor)
      {
      }

      public ServiceMapping(
         Option<BaseUnitOfWork> uow,
         Option<BaseRepository<T>> repository,
         Option<TInterceptor> interceptor)
      {
         Uow = uow;
         Repository = repository;
         Interceptor = interceptor;
      }

      #endregion

      #region Properties

      internal Option<TInterceptor> Interceptor { get; }
      internal Option<BaseRepository<T>> Repository { get; }
      internal Option<BaseUnitOfWork> Uow { get; }

      #endregion

      #region Methods

      internal Option<ServiceMapping<T, TInterceptor2>> ChangeInterceptor<TInterceptor2>()
         where TInterceptor2 : IInterceptor
      {
         return new ServiceMapping<T, TInterceptor2>(Uow, Repository, Interceptor.ReduceOrDefault().As<TInterceptor2>());
      }

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Uow;
         yield return Repository;
         yield return Interceptor;
      }

      #endregion
   }
}