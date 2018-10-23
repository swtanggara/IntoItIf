namespace IntoItIf.Base.Services.Mediator.Handlers
{
   using System.Collections.Generic;
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

   public sealed class ReadLookupHandler<T, TDto, TReadLookupInterceptor, TRepository>
      : BaseRequestHandler<T, TReadLookupInterceptor, TRepository>, IRequestHandler<ReadLookupRequest<T, TDto>, List<KeyValue>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadLookupInterceptor : IReadLookupInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public ReadLookupHandler(Option<IUow> uow) : base(uow)
      {
      }

      public ReadLookupHandler(Option<IUow> uow, Option<TReadLookupInterceptor> interceptor) : base(uow, interceptor)
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