namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Entities.Interceptors;
   using Handlers;
   using Requests;

   public sealed class ReadLookup<T, TDto, TReadLookupInterceptor>
      : RequestHandlerFor<T, TReadLookupInterceptor, ReadLookupHandler<T, TDto, TReadLookupInterceptor>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadLookupInterceptor : IReadLookupInterceptor
   {
      #region Public Methods and Operators

      public static Task<Option<List<KeyValue>>> Handle(Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         var @for = new ReadLookup<T, TDto, TReadLookupInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new ReadLookupRequest<T, TDto>(useValueAsId), ctok);
      }

      #endregion
   }
}