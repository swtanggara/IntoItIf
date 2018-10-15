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

   public sealed class CrudHandler<T, TDto, TCrudInterceptor> : BaseRequestHandler<T, TCrudInterceptor>, ICrudHandler<T, TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TCrudInterceptor : ICrudInterceptor
   {
      #region Constructors and Destructors

      public CrudHandler()
      {
      }

      public CrudHandler(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public CrudHandler(Option<BaseUnitOfWork> uow, Option<TCrudInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<Dictionary<string, object>>> Handle(
         Option<CreateRequest<T, TDto>> request,
         Option<CancellationToken> ctok)
      {
         return ServiceMapping.CreateEntityAsync(request.ReduceOrDefault().Dto, ctok);
      }

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
         return ServiceMapping.GetPagedAsync<T, TDto, TCrudInterceptor>(x.PageNo, x.PageSize, x.Sorts, x.Keyword, ctok);
      }

      public Task<Option<Dictionary<string, object>>> Handle(
         Option<UpdateRequest<T, TDto>> request,
         Option<CancellationToken> ctok)
      {
         return ServiceMapping.UpdateEntityAsync(request.ReduceOrDefault().Dto, ctok);
      }

      public Task<Option<bool>> Handle(Option<DeleteRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.DeleteEntityAsync(request.ReduceOrDefault().Criteria, ctok);
      }

      #endregion
   }
}