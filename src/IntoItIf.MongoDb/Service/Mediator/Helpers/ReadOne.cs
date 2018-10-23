namespace IntoItIf.MongoDb.Service.Mediator.Helpers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Services.Entities.Interceptors;
   using Base.Services.Mediator.Helpers;

   public class ReadOne<T, TDto, TReadOneInterceptor> : ReadOne<T, TDto, TReadOneInterceptor, MongoRepository<T>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadOneInterceptor : IReadOneInterceptor
   {
      #region Constructors and Destructors

      public ReadOne(Option<MongoUow> uow) : base(uow.ReduceOrDefault())
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<TDto>> Handle(Option<MongoUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new ReadOne<T, TDto, TReadOneInterceptor>(uow);
         return @for.Handle(criteria, ctok);
      }

      #endregion
   }
}