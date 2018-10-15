namespace IntoItIf.Dsl.Entities.Services
{
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;
   using Helpers;
   using Interceptors;

   public sealed class EntityDeleteService<T, TDto, TDeleteInterceptor>
      : BaseEntityService<T, TDeleteInterceptor>, IEntityDeleteService<TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TDeleteInterceptor : IDeleteInterceptor
   {
      #region Constructors and Destructors

      public EntityDeleteService()
      {
      }

      public EntityDeleteService(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public EntityDeleteService(Option<BaseUnitOfWork> uow, Option<TDeleteInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Option<bool> DeleteEntity(Option<TDto> criteria)
      {
         return ServiceMapping.DeleteEntity(criteria);
      }

      public Task<Option<bool>> DeleteEntityAsync(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         return ServiceMapping.DeleteEntityAsync(criteria, ctok);
      }

      #endregion
   }
}