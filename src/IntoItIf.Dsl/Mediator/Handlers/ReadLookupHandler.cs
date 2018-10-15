namespace IntoItIf.Dsl.Mediator.Handlers
{
   using System.Collections.Generic;
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

   public sealed class ReadLookupHandler<T, TDto, TReadLookupInterceptor>
      : BaseRequestHandler<T, TReadLookupInterceptor>, IRequestHandler<ReadLookupRequest<T, TDto>, List<KeyValue>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadLookupInterceptor : IReadLookupInterceptor
   {
      #region Constructors and Destructors

      public ReadLookupHandler()
      {
      }

      public ReadLookupHandler(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public ReadLookupHandler(Option<BaseUnitOfWork> uow, Option<TReadLookupInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<List<KeyValue>>> Handle(Option<ReadLookupRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetLookupsAsync(request.ReduceOrDefault().UseValueAsId, ctok);
      }

      #endregion
   }
}