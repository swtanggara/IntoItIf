namespace IntoItIf.Dsl.Mediator.Handlers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Mediator;
   using Dal.UnitOfWorks;
   using Entities.Helpers;
   using Entities.Interceptors;
   using Requests;

   public sealed class CreateHandler<T, TDto, TCreateInterceptor>
      : BaseRequestHandler<T, TCreateInterceptor>, IRequestHandler<CreateRequest<T, TDto>, Dictionary<string, object>>
      where T : class, IEntity
      where TDto : class, IDto
      where TCreateInterceptor : ICreateInterceptor
   {
      #region Constructors and Destructors

      public CreateHandler()
      {
      }

      public CreateHandler(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public CreateHandler(Option<BaseUnitOfWork> uow, Option<TCreateInterceptor> interceptor) : base(uow, interceptor)
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