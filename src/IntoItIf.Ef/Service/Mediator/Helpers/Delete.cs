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

   public class Delete<T, TDto, TDeleteInterceptor> : Delete<T, TDto, TDeleteInterceptor, EfRepository<T>>
      where T : class, IEntity
      where TDto : class, IDto
      where TDeleteInterceptor : IDeleteInterceptor
   {
      #region Constructors and Destructors

      public Delete(Option<EfUow> uow) : base(uow.ReduceOrDefault())
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<bool>> Handle(Option<EfUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Delete<T, TDto, TDeleteInterceptor>(uow);
         return @for.Handle(criteria, ctok);
      }

      #endregion
   }
}