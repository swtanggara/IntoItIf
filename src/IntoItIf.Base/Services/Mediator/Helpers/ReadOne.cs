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

   public class ReadOne<T, TDto, TReadOneInterceptor, TRepository>
      : RequestHandlerFor<T, TReadOneInterceptor, ReadOneHandler<T, TDto, TReadOneInterceptor, TRepository>, TRepository>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadOneInterceptor : IReadOneInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public ReadOne(Option<IUow> uow) : base(uow)
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<TDto>> Handle(Option<IUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new ReadOne<T, TDto, TReadOneInterceptor, TRepository>(uow);
         return @for.Handle(criteria, ctok);
      }

      public Task<Option<TDto>> Handle(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().Handle(new ReadOneRequest<T, TDto>(criteria), ctok);
      }

      #endregion
   }
}