namespace IntoItIf.Ef.Service.Mediator.Helpers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Services.Entities.Interceptors;
   using Base.Services.Mediator.Helpers;
   using Repositories;
   using UnitOfWork;

   public class ReadOne<T, TDto, TReadOneInterceptor> : ReadOne<T, TDto, TReadOneInterceptor, EfRepository<T>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadOneInterceptor : IReadOneInterceptor
   {
      #region Constructors and Destructors

      public ReadOne(Option<EfUow> uow) : base(uow.ReduceOrDefault())
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<TDto>> Handle(Option<EfUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new ReadOne<T, TDto, TReadOneInterceptor>(uow);
         return @for.Handle(criteria, ctok);
      }

      #endregion
   }
}