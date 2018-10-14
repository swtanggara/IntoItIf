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

   public sealed class Read<T, TDto, TReadInterceptor>
      : RequestHandlerFor<T, TReadInterceptor, ReadHandler<T, TDto, TReadInterceptor>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadInterceptor : IReadInterceptor
   {
      #region Public Methods and Operators

      public static Task<Option<List<KeyValue>>> Handle(Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         var @for = new Read<T, TDto, TReadInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new ReadLookupRequest<T, TDto>(useValueAsId), ctok);
      }

      public static Task<Option<TDto>> Handle(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Read<T, TDto, TReadInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new ReadOneRequest<T, TDto>(criteria), ctok);
      }

      public static Task<Option<IPaged<TDto>>> Handle(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         var @for = new Read<T, TDto, TReadInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new ReadPagedRequest<T, TDto>(pageNo, pageSize, sorts, keyword), ctok);
      }

      #endregion
   }
}