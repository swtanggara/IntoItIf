namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Entities.Interceptors;
   using Handlers;
   using Requests;

   public sealed class Update<T, TDto, TUpdateInterceptor>
      : RequestHandlerFor<T, TUpdateInterceptor, UpdateHandler<T, TDto, TUpdateInterceptor>>
      where T : class, IEntity
      where TDto : class, IDto
      where TUpdateInterceptor : IUpdateInterceptor
   {
      #region Public Methods and Operators

      public static Task<Option<Dictionary<string, object>>> Handle(Option<TDto> dto, Option<CancellationToken> ctok)
      {
         var @for = new Update<T, TDto, TUpdateInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new UpdateRequest<T, TDto>(dto), ctok);
      }

      #endregion
   }
}