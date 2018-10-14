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

   public sealed class Crud<T, TDto, TCrudInterceptor>
      : RequestHandlerFor<T, TCrudInterceptor, CrudHandler<T, TDto, TCrudInterceptor>>
      where T : class, IEntity
      where TDto : class, IDto
      where TCrudInterceptor : ICrudInterceptor
   {
      #region Public Methods and Operators

      public static Task<Option<Dictionary<string, object>>> HandleCreate(Option<TDto> dto, Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new CreateRequest<T, TDto>(dto), ctok);
      }

      public static Task<Option<bool>> HandleDelete(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new DeleteRequest<T, TDto>(criteria), ctok);
      }

      public static Task<Option<List<KeyValue>>> HandleReadLookup(Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new ReadLookupRequest<T, TDto>(useValueAsId), ctok);
      }

      public static Task<Option<TDto>> HandleReadOne(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new ReadOneRequest<T, TDto>(criteria), ctok);
      }

      public static Task<Option<IPaged<TDto>>> HandleReadPaged(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new ReadPagedRequest<T, TDto>(pageNo, pageSize, sorts, keyword), ctok);
      }

      public static Task<Option<Dictionary<string, object>>> HandleUpdate(Option<TDto> dto, Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new UpdateRequest<T, TDto>(dto), ctok);
      }

      #endregion
   }
}