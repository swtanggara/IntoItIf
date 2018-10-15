namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Entities.Interceptors;
   using Handlers;
   using Requests;

   public sealed class Delete<T, TDto, TDeleteInterceptor>
      : RequestHandlerFor<T, TDeleteInterceptor, DeleteHandler<T, TDto, TDeleteInterceptor>>
      where T : class, IEntity
      where TDto : class, IDto
      where TDeleteInterceptor : IDeleteInterceptor
   {
      #region Public Methods and Operators

      public static Task<Option<bool>> Handle(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Delete<T, TDto, TDeleteInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new DeleteRequest<T, TDto>(criteria), ctok);
      }

      #endregion
   }
}