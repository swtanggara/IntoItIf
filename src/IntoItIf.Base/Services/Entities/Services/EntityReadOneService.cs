namespace IntoItIf.Base.Services.Entities.Services
{
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Entities;
   using Domain.Options;
   using Helpers;
   using Interceptors;
   using Repositories;
   using UnitOfWork;

   public sealed class EntityReadOneService<T, TDto, TReadOneInterceptor, TRepository>
      : BaseEntityService<T, TReadOneInterceptor, TRepository>, IEntityReadOneService<TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadOneInterceptor : IReadOneInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public EntityReadOneService()
      {
      }

      public EntityReadOneService(Option<ISaveUow> uow) : base(uow)
      {
      }

      public EntityReadOneService(Option<ISaveUow> uow, Option<TReadOneInterceptor> interceptor) : base(uow, interceptor)
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