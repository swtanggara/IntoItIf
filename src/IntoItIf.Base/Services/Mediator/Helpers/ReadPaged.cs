namespace IntoItIf.Base.Services.Mediator.Helpers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;
   using Domain.Entities;
   using Domain.Options;
   using Entities.Interceptors;
   using Handlers;
   using Repositories;
   using Requests;
   using UnitOfWork;

   public class ReadPaged<T, TDto, TReadPagedInterceptor, TRepository>
      : RequestHandlerFor<T, TReadPagedInterceptor, ReadPagedHandler<T, TDto, TReadPagedInterceptor, TRepository>, TRepository>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadPagedInterceptor : IReadPagedInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public ReadPaged(Option<IUow> uow) : base(uow)
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<IPaged<TDto>>> Handle(
         Option<IUow> uow,
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         var @for = new ReadPaged<T, TDto, TReadPagedInterceptor, TRepository>(uow);
         return @for.Handle(pageNo, pageSize, sorts, keyword, ctok);
      }

      public Task<Option<IPaged<TDto>>> Handle(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().HandleAsync(new ReadPagedRequest<T, TDto>(pageNo, pageSize, sorts, keyword), ctok);
      }

      #endregion
   }
}