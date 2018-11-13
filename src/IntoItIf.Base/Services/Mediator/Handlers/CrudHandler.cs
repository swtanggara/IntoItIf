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

   public sealed class CrudHandler<T, TDto, TCrudInterceptor, TRepository>
      : BaseRequestHandler<T, TCrudInterceptor, TRepository>, ICrudHandler<T, TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TCrudInterceptor : ICrudInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public CrudHandler(Option<ISaveUow> uow) : base(uow)
      {
      }

      public CrudHandler(Option<ISaveUow> uow, Option<TCrudInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<Dictionary<string, object>>> HandleAsync(
         Option<CreateRequest<T, TDto>> request,
         Option<CancellationToken> ctok)
      {
         return ServiceMapping.CreateEntityAsync(request.ReduceOrDefault().Dto, ctok);
      }

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
         return ServiceMapping.GetPagedAsync<T, TDto, TCrudInterceptor, TRepository>(x.PageNo, x.PageSize, x.Sorts, x.Keyword, ctok);
      }

      public Task<Option<Dictionary<string, object>>> HandleAsync(
         Option<UpdateRequest<T, TDto>> request,
         Option<CancellationToken> ctok)
      {
         return ServiceMapping.UpdateEntityAsync(request.ReduceOrDefault().Dto, ctok);
      }

      public Task<Option<bool>> HandleAsync(Option<DeleteRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.DeleteEntityAsync(request.ReduceOrDefault().Criteria, ctok);
      }

      #endregion
   }
}