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

   public sealed class DeleteHandler<T, TDto, TDeleteInterceptor>
      : BaseRequestHandler<T, TDeleteInterceptor>, IRequestHandler<DeleteRequest<T, TDto>>
      where T : class, IEntity
      where TDto : class, IDto
      where TDeleteInterceptor : IDeleteInterceptor
   {
      #region Constructors and Destructors

      public DeleteHandler()
      {
      }

      public DeleteHandler(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public DeleteHandler(Option<BaseUnitOfWork> uow, Option<TDeleteInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<bool>> Handle(Option<DeleteRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.DeleteEntityAsync(request.ReduceOrDefault().Criteria, ctok);
      }

      #endregion
   }
}