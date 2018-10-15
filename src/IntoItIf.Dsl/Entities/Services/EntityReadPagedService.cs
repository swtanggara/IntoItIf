namespace IntoItIf.Dsl.Entities.Services
{
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;
   using Helpers;
   using Interceptors;

   public sealed class EntityReadPagedService<T, TDto, TReadPagedInterceptor>
      : BaseEntityService<T, TReadPagedInterceptor>, IEntityReadPagedService<TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadPagedInterceptor : IReadPagedInterceptor
   {
      #region Constructors and Destructors

      public EntityReadPagedService()
      {
      }

      public EntityReadPagedService(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public EntityReadPagedService(Option<BaseUnitOfWork> uow, Option<TReadPagedInterceptor> interceptor) : base(
         uow,
         interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Option<IPaged<TDto>> GetPaged(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword)
      {
         return ServiceMapping.GetPaged<T, TDto, TReadPagedInterceptor>(pageNo, pageSize, sorts, keyword);
      }

      public Task<Option<IPaged<TDto>>> GetPagedAsync(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetPagedAsync<T, TDto, TReadPagedInterceptor>(pageNo, pageSize, sorts, keyword, ctok);
      }

      #endregion
   }
}