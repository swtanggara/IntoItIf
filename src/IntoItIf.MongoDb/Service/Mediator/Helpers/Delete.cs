namespace IntoItIf.MongoDb.Service.Mediator.Helpers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Services.Entities.Interceptors;
   using Base.Services.Mediator.Helpers;

   public class Delete<T, TDto, TDeleteInterceptor> : Delete<T, TDto, TDeleteInterceptor, MongoRepository<T>>
      where T : class, IMongoEntity
      where TDto : class, IDto
      where TDeleteInterceptor : IDeleteInterceptor
   {
      #region Constructors and Destructors

      public Delete(Option<MongoUow> uow) : base(uow.ReduceOrDefault())
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<bool>> Handle(Option<MongoUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Delete<T, TDto, TDeleteInterceptor>(uow);
         return @for.Handle(criteria, ctok);
      }

      #endregion
   }
}