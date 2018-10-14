namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Entities.Interceptors;
   using Handlers;
   using Requests;

   public sealed class ReadPaged<T, TDto, TReadPagedInterceptor>
      : RequestHandlerFor<T, TReadPagedInterceptor, ReadPagedHandler<T, TDto, TReadPagedInterceptor>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadPagedInterceptor : IReadPagedInterceptor
   {
      #region Public Methods and Operators

      public static Task<Option<IPaged<TDto>>> Handle(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         var @for = new ReadPaged<T, TDto, TReadPagedInterceptor>();
         return @for.Handler().ReduceOrDefault().Handle(new ReadPagedRequest<T, TDto>(pageNo, pageSize, sorts, keyword), ctok);
      }

      #endregion
   }
}