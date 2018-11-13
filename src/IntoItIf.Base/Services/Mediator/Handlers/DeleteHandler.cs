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

   public sealed class DeleteHandler<T, TDto, TDeleteInterceptor, TRepository>
      : BaseRequestHandler<T, TDeleteInterceptor, TRepository>, IRequestHandler<DeleteRequest<T, TDto>>
      where T : class, IEntity
      where TDto : class, IDto
      where TDeleteInterceptor : IDeleteInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public DeleteHandler(Option<ISaveUow> uow) : base(uow)
      {
      }

      public DeleteHandler(Option<ISaveUow> uow, Option<TDeleteInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<bool>> HandleAsync(Option<DeleteRequest<T, TDto>> request, Option<CancellationToken> ctok)
      {
         return ServiceMapping.DeleteEntityAsync(request.ReduceOrDefault().Criteria, ctok);
      }

      #endregion
   }
}