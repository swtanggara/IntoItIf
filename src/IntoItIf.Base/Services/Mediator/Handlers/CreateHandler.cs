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

   public sealed class CreateHandler<T, TDto, TCreateInterceptor, TRepository>
      : BaseRequestHandler<T, TCreateInterceptor, TRepository>, IRequestHandler<CreateRequest<T, TDto>, Dictionary<string, object>>
      where T : class, IEntity
      where TDto : class, IDto
      where TCreateInterceptor : ICreateInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public CreateHandler(Option<IUow> uow) : base(uow)
      {
      }

      public CreateHandler(Option<IUow> uow, Option<TCreateInterceptor> interceptor) : base(uow, interceptor)
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

      #endregion
   }
}