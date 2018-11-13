namespace IntoItIf.Base.Services.Mediator.Handlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Mediator;
   using Domain.Entities;
   using Domain.Options;
   using Entities.Helpers;
   using Entities.Interceptors;
   using Repositories;
   using Requests;
   using UnitOfWork;

   public sealed class ReadOneHandler<T, TDto, TReadOneInterceptor, TRepository>
      : BaseRequestHandler<T, TReadOneInterceptor, TRepository>, IRequestHandler<ReadOneRequest<T, TDto>, TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadOneInterceptor : IReadOneInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public ReadOneHandler(Option<ISaveUow> uow) : base(uow)
      {
      }

      public ReadOneHandler(Option<ISaveUow> uow, Option<TReadOneInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<TDto>> HandleAsync(Option<ReadOneRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetByPredicateAsync(request.ReduceOrDefault().Criteria, ctok);
      }

      #endregion
   }
}