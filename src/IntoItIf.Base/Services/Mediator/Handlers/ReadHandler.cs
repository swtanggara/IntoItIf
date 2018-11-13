namespace IntoItIf.Base.Services.Mediator.Handlers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;
   using Domain.Entities;
   using Domain.Options;
   using Entities.Helpers;
   using Entities.Interceptors;
   using Repositories;
   using Requests;
   using UnitOfWork;

   public sealed class ReadHandler<T, TDto, TReadInterceptor, TRepository>
      : BaseRequestHandler<T, TReadInterceptor, TRepository>, IReadHandler<T, TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadInterceptor : IReadInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public ReadHandler(Option<ISaveUow> uow) : base(uow)
      {
      }

      public ReadHandler(Option<ISaveUow> uow, Option<TReadInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<List<KeyValue>>> HandleAsync(Option<ReadLookupRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetLookupsAsync(request.ReduceOrDefault().UseValueAsId, ctok);
      }

      public Task<Option<TDto>> HandleAsync(Option<ReadOneRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetByPredicateAsync(request.ReduceOrDefault().Criteria, ctok);
      }

      public Task<Option<IPaged<TDto>>> HandleAsync(Option<ReadPagedRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         var x = request.ReduceOrDefault();
         return ServiceMapping.GetPagedAsync<T, TDto, TReadInterceptor, TRepository>(x.PageNo, x.PageSize, x.Sorts, x.Keyword, ctok);
      }

      #endregion
   }
}