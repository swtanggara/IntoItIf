namespace IntoItIf.Dsl.Entities.Services
{
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;
   using Helpers;
   using Interceptors;

   public sealed class EntityReadOneService<T, TDto, TReadOneInterceptor>
      : BaseEntityService<T, TReadOneInterceptor>, IEntityReadOneService<TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadOneInterceptor : IReadOneInterceptor
   {
      #region Constructors and Destructors

      public EntityReadOneService()
      {
      }

      public EntityReadOneService(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public EntityReadOneService(Option<BaseUnitOfWork> uow, Option<TReadOneInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Option<TDto> GetByPredicate(Option<TDto> criteria)
      {
         return ServiceMapping.GetByPredicate(criteria);
      }

      public Task<Option<TDto>> GetByPredicateAsync(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetByPredicateAsync(criteria, ctok);
      }

      public Option<bool> IsExist(Option<TDto> criteria)
      {
         return GetByPredicate(criteria).Map(x => x != null);
      }

      public Task<Option<bool>> IsExistAsync(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         return GetByPredicateAsync(criteria, ctok).MapAsync(x => Task.FromResult(x != null));
      }

      #endregion
   }
}