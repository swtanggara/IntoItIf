namespace IntoItIf.Base.Services.Mediator.Helpers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Entities;
   using Domain.Options;
   using Entities.Interceptors;
   using Handlers;
   using Repositories;
   using Requests;
   using UnitOfWork;

   public class Update<T, TDto, TUpdateInterceptor, TRepository>
      : RequestHandlerFor<T, TUpdateInterceptor, UpdateHandler<T, TDto, TUpdateInterceptor, TRepository>, TRepository>
      where T : class, IEntity
      where TDto : class, IDto
      where TUpdateInterceptor : IUpdateInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public Update(Option<IUow> uow) : base(uow)
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<Dictionary<string, object>>> Handle(Option<IUow> uow, Option<TDto> dto, Option<CancellationToken> ctok)
      {
         var @for = new Update<T, TDto, TUpdateInterceptor, TRepository>(uow);
         return @for.Handle(dto, ctok);
      }

      public Task<Option<Dictionary<string, object>>> Handle(Option<TDto> dto, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().HandleAsync(new UpdateRequest<T, TDto>(dto), ctok);
      }

      #endregion
   }
}