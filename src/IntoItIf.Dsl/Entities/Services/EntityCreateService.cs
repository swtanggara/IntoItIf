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

   public sealed class EntityCreateService<T, TDto, TCreateInterceptor>
      : BaseEntityService<T, TCreateInterceptor>, IEntityCreateService<TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TCreateInterceptor : ICreateInterceptor
   {
      #region Constructors and Destructors

      public EntityCreateService()
      {
      }

      public EntityCreateService(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public EntityCreateService(Option<BaseUnitOfWork> uow, Option<TCreateInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Option<Dictionary<string, object>> CreateEntity(Option<TDto> dto)
      {
         return ServiceMapping.CreateEntity(dto);
      }

      public Task<Option<Dictionary<string, object>>> CreateEntityAsync(Option<TDto> dto, Option<CancellationToken> ctok)
      {
         return ServiceMapping.CreateEntityAsync(dto, ctok);
      }

      #endregion
   }
}