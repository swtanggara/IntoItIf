namespace IntoItIf.Base.Services.Entities.Services
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Entities;
   using Domain.Options;
   using Helpers;
   using Interceptors;
   using Repositories;
   using UnitOfWork;

   public sealed class EntityCreateService<T, TDto, TCreateInterceptor, TRepository>
      : BaseEntityService<T, TCreateInterceptor, TRepository>, IEntityCreateService<TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TCreateInterceptor : ICreateInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public EntityCreateService()
      {
      }

      public EntityCreateService(Option<ISaveUow> uow) : base(uow)
      {
      }

      public EntityCreateService(Option<ISaveUow> uow, Option<TCreateInterceptor> interceptor) : base(uow, interceptor)
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