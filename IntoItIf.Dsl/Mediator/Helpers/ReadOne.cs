namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Entities.Interceptors;
   using Handlers;
   using Requests;

   public sealed class ReadOne<T, TDto, TReadOneInterceptor>
      : RequestHandlerFor<T, TReadOneInterceptor, ReadOneHandler<T, TDto, TReadOneInterceptor>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadOneInterceptor : IReadOneInterceptor
   {
      #region Public Methods and Operators

      public static Task<Option<TDto>> Handle(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new ReadOne<T, TDto, TReadOneInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new ReadOneRequest<T, TDto>(criteria), ctok);
      }

      #endregion
   }
}