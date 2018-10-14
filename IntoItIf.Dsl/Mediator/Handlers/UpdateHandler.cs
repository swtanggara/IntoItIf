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

   public sealed class UpdateHandler<T, TDto, TUpdateInterceptor>
      : BaseRequestHandler<T, TUpdateInterceptor>, IRequestHandler<UpdateRequest<T, TDto>, Dictionary<string, object>>
      where T : class, IEntity
      where TDto : class, IDto
      where TUpdateInterceptor : IUpdateInterceptor
   {
      #region Constructors and Destructors

      public UpdateHandler()
      {
      }

      public UpdateHandler(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public UpdateHandler(Option<BaseUnitOfWork> uow, Option<TUpdateInterceptor> interceptor) : base(uow, interceptor)
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