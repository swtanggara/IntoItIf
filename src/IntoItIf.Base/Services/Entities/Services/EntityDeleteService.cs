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

   public sealed class EntityDeleteService<T, TDto, TDeleteInterceptor, TRepository>
      : BaseEntityService<T, TDeleteInterceptor, TRepository>, IEntityDeleteService<TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TDeleteInterceptor : IDeleteInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public EntityDeleteService()
      {
      }

      public EntityDeleteService(Option<ISaveUow> uow) : base(uow)
      {
      }

      public EntityDeleteService(Option<ISaveUow> uow, Option<TDeleteInterceptor> interceptor) : base(uow, interceptor)
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