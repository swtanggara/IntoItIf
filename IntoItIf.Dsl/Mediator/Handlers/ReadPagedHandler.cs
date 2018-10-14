namespace IntoItIf.Dsl.Mediator.Handlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Mediator;
   using Dal.UnitOfWorks;
   using Entities.Helpers;
   using Entities.Interceptors;
   using Requests;

   public sealed class ReadPagedHandler<T, TDto, TReadPagedInterceptor>
      : BaseRequestHandler<T, TReadPagedInterceptor>, IRequestHandler<ReadPagedRequest<T, TDto>, IPaged<TDto>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadPagedInterceptor : IReadPagedInterceptor
   {
      #region Constructors and Destructors

      public ReadPagedHandler()
      {
      }

      public ReadPagedHandler(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public ReadPagedHandler(Option<BaseUnitOfWork> uow, Option<TReadPagedInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<IPaged<TDto>>> Handle(Option<ReadPagedRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         var x = request.ReduceOrDefault();
         return ServiceMapping.GetPagedAsync<T, TDto, TReadPagedInterceptor>(x.PageNo, x.PageSize, x.Sorts, x.Keyword, ctok);
      }

      #endregion
   }
}