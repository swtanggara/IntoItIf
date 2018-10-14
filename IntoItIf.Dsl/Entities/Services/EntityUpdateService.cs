namespace IntoItIf.Dsl.Entities.Services
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;
   using Helpers;
   using Interceptors;

   public sealed class EntityUpdateService<T, TDto, TUpdateInterceptor>
      : BaseEntityService<T, TUpdateInterceptor>, IEntityUpdateService<TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TUpdateInterceptor : IUpdateInterceptor
   {
      #region Constructors and Destructors

      public EntityUpdateService()
      {
      }

      public EntityUpdateService(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public EntityUpdateService(Option<BaseUnitOfWork> uow, Option<TUpdateInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Option<Dictionary<string, object>> UpdateEntity(Option<TDto> dto)
      {
         return ServiceMapping.UpdateEntity(dto);
      }

      public Task<Option<Dictionary<string, object>>> UpdateEntityAsync(Option<TDto> dto, Option<CancellationToken> ctok)
      {
         return ServiceMapping.UpdateEntityAsync(dto, ctok);
      }

      #endregion
   }
}