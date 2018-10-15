namespace IntoItIf.Dsl.Mediator.Handlers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;
   using Entities.Helpers;
   using Entities.Interceptors;
   using Requests;

   public sealed class ReadHandler<T, TDto, TReadInterceptor> : BaseRequestHandler<T, TReadInterceptor>, IReadHandler<T, TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadInterceptor : IReadInterceptor
   {
      #region Constructors and Destructors

      public ReadHandler()
      {
      }

      public ReadHandler(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public ReadHandler(Option<BaseUnitOfWork> uow, Option<TReadInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<List<KeyValue>>> Handle(Option<ReadLookupRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetLookupsAsync(request.ReduceOrDefault().UseValueAsId, ctok);
      }

      public Task<Option<TDto>> Handle(Option<ReadOneRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetByPredicateAsync(request.ReduceOrDefault().Criteria, ctok);
      }

      public Task<Option<IPaged<TDto>>> Handle(Option<ReadPagedRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         var x = request.ReduceOrDefault();
         return ServiceMapping.GetPagedAsync<T, TDto, TReadInterceptor>(x.PageNo, x.PageSize, x.Sorts, x.Keyword, ctok);
      }

      #endregion
   }
}