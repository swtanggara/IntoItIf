namespace IntoItIf.Base.Services.Mediator.Handlers
{
   using System.Collections.Generic;
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

   public sealed class UpdateHandler<T, TDto, TUpdateInterceptor, TRepository>
      : BaseRequestHandler<T, TUpdateInterceptor, TRepository>, IRequestHandler<UpdateRequest<T, TDto>, Dictionary<string, object>>
      where T : class, IEntity
      where TDto : class, IDto
      where TUpdateInterceptor : IUpdateInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public UpdateHandler(Option<IUow> uow) : base(uow)
      {
      }

      public UpdateHandler(Option<IUow> uow, Option<TUpdateInterceptor> interceptor) : base(uow, interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<Dictionary<string, object>>> Handle(
         Option<UpdateRequest<T, TDto>> request,
         Option<CancellationToken> ctok)
      {
         return ServiceMapping.UpdateEntityAsync(request.ReduceOrDefault().Dto, ctok);
      }

      #endregion
   }
}