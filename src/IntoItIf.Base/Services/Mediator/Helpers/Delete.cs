namespace IntoItIf.Base.Services.Mediator.Helpers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Entities;
   using Domain.Options;
   using Entities.Interceptors;
   using Handlers;
   using Repositories;
   using Requests;
   using UnitOfWork;

   public class Delete<T, TDto, TDeleteInterceptor, TRepository>
      : RequestHandlerFor<T, TDeleteInterceptor, DeleteHandler<T, TDto, TDeleteInterceptor, TRepository>, TRepository>
      where T : class, IEntity
      where TDto : class, IDto
      where TDeleteInterceptor : IDeleteInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public Delete(Option<IUow> uow) : base(uow)
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<bool>> Handle(Option<IUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Delete<T, TDto, TDeleteInterceptor, TRepository>(uow);
         return @for.Handle(criteria, ctok);
      }

      public Task<Option<bool>> Handle(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().HandleAsync(new DeleteRequest<T, TDto>(criteria), ctok);
      }

      #endregion
   }
}