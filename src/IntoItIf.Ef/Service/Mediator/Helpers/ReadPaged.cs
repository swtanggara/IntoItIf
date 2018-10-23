namespace IntoItIf.Ef.Service.Mediator.Helpers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Services.Entities.Interceptors;
   using Base.Services.Mediator.Helpers;
   using Repositories;
   using UnitOfWork;

   public class ReadPaged<T, TDto, TReadPagedInterceptor> : ReadPaged<T, TDto, TReadPagedInterceptor, EfRepository<T>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadPagedInterceptor : IReadPagedInterceptor
   {
      #region Constructors and Destructors

      public ReadPaged(Option<EfUow> uow) : base(uow.ReduceOrDefault())
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<IPaged<TDto>>> Handle(
         Option<EfUow> uow,
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         var @for = new ReadPaged<T, TDto, TReadPagedInterceptor>(uow);
         return @for.Handle(pageNo, pageSize, sorts, keyword, ctok);
      }

      #endregion
   }
}