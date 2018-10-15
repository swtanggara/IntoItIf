namespace IntoItIf.Dsl.Mediator.Handlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Mediator;
   using Dal.UnitOfWorks;
   using Entities.Helpers;
   using Entities.Interceptors;
   using Requests;

   public sealed class ReadOneHandler<T, TDto, TReadOneInterceptor>
      : BaseRequestHandler<T, TReadOneInterceptor>, IRequestHandler<ReadOneRequest<T, TDto>, TDto>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadOneInterceptor : IReadOneInterceptor
   {
      #region Constructors and Destructors

      public ReadOneHandler()
      {
      }

      public ReadOneHandler(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public ReadOneHandler(Option<BaseUnitOfWork> uow, Option<TReadOneInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<TDto>> Handle(Option<ReadOneRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetByPredicateAsync(request.ReduceOrDefault().Criteria, ctok);
      }

      #endregion
   }
}