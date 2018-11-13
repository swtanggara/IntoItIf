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

   public sealed class EntityUpdateService<T, TDto, TUpdateInterceptor, TRepository>
      : BaseEntityService<T, TUpdateInterceptor, TRepository>, IEntityUpdateService<TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TUpdateInterceptor : IUpdateInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public EntityUpdateService()
      {
      }

      public EntityUpdateService(Option<ISaveUow> uow) : base(uow)
      {
      }

      public EntityUpdateService(Option<ISaveUow> uow, Option<TUpdateInterceptor> interceptor) : base(uow, interceptor)
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