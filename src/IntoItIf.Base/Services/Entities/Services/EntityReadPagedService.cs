namespace IntoItIf.Base.Services.Entities.Services
{
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;
   using Domain.Entities;
   using Domain.Options;
   using Helpers;
   using Interceptors;
   using Repositories;
   using UnitOfWork;

   public sealed class EntityReadPagedService<T, TDto, TReadPagedInterceptor, TRepository>
      : BaseEntityService<T, TReadPagedInterceptor, TRepository>, IEntityReadPagedService<TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadPagedInterceptor : IReadPagedInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public EntityReadPagedService()
      {
      }

      public EntityReadPagedService(Option<ISaveUow> uow) : base(uow)
      {
      }

      public EntityReadPagedService(Option<ISaveUow> uow, Option<TReadPagedInterceptor> interceptor) : base(
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
         return ServiceMapping.GetPaged<T, TDto, TReadPagedInterceptor, TRepository>(pageNo, pageSize, sorts, keyword);
      }

      public Task<Option<IPaged<TDto>>> GetPagedAsync(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetPagedAsync<T, TDto, TReadPagedInterceptor, TRepository>(pageNo, pageSize, sorts, keyword, ctok);
      }

      #endregion
   }
}