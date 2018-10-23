namespace IntoItIf.MongoDb.Service.Mediator.Helpers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Services.Entities.Interceptors;
   using Base.Services.Mediator.Helpers;

   public class Update<T, TDto, TUpdateInterceptor> : Update<T, TDto, TUpdateInterceptor, MongoRepository<T>>
      where T : class, IEntity
      where TDto : class, IDto
      where TUpdateInterceptor : IUpdateInterceptor
   {
      #region Constructors and Destructors

      public Update(Option<MongoUow> uow) : base(uow.ReduceOrDefault())
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<Dictionary<string, object>>> Handle(Option<MongoUow> uow, Option<TDto> dto, Option<CancellationToken> ctok)
      {
         var @for = new Update<T, TDto, TUpdateInterceptor>(uow);
         return @for.Handle(dto, ctok);
      }

      #endregion
   }
}