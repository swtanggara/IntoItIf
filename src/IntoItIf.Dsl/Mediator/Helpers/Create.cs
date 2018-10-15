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

   public class Create<T, TDto, TCreateInterceptor>
      : RequestHandlerFor<T, TCreateInterceptor, CreateHandler<T, TDto, TCreateInterceptor>>
      where T : class, IEntity
      where TDto : class, IDto
      where TCreateInterceptor : ICreateInterceptor
   {
      #region Public Methods and Operators

      public static Task<Option<Dictionary<string, object>>> Handle(Option<TDto> dto, Option<CancellationToken> ctok)
      {
         var @for = new Create<T, TDto, TCreateInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new CreateRequest<T, TDto>(dto), ctok);
      }

      #endregion
   }
}