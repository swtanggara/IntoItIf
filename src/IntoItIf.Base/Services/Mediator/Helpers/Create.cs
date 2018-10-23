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

   public class Create<T, TDto, TCreateInterceptor, TRepository>
      : RequestHandlerFor<T, TCreateInterceptor, CreateHandler<T, TDto, TCreateInterceptor, TRepository>, TRepository>
      where T : class, IEntity
      where TDto : class, IDto
      where TCreateInterceptor : ICreateInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public Create(Option<IUow> uow) : base(uow)
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<Dictionary<string, object>>> Handle(Option<IUow> uow, Option<TDto> dto, Option<CancellationToken> ctok)
      {
         var @for = new Create<T, TDto, TCreateInterceptor, TRepository>(uow);
         return @for.Handle(dto, ctok);
      }

      public Task<Option<Dictionary<string, object>>> Handle(Option<TDto> dto, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().Handle(new CreateRequest<T, TDto>(dto), ctok);
      }

      #endregion
   }
}