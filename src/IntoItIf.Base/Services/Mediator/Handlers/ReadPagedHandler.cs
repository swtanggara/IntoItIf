namespace IntoItIf.Base.Services.Mediator.Handlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Mediator;
   using Domain;
   using Domain.Entities;
   using Domain.Options;
   using Entities.Helpers;
   using Entities.Interceptors;
   using Repositories;
   using Requests;
   using UnitOfWork;

   public sealed class ReadPagedHandler<T, TDto, TReadPagedInterceptor, TRepository>
      : BaseRequestHandler<T, TReadPagedInterceptor, TRepository>, IRequestHandler<ReadPagedRequest<T, TDto>, IPaged<TDto>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadPagedInterceptor : IReadPagedInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public ReadPagedHandler(Option<IUow> uow) : base(uow)
      {
      }

      public ReadPagedHandler(Option<IUow> uow, Option<TReadPagedInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<IPaged<TDto>>> Handle(Option<ReadPagedRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         var x = request.ReduceOrDefault();
         return ServiceMapping.GetPagedAsync<T, TDto, TReadPagedInterceptor, TRepository>(x.PageNo, x.PageSize, x.Sorts, x.Keyword, ctok);
      }

      #endregion
   }
}