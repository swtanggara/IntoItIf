namespace IntoItIf.Base.Services.Mediator.Helpers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;
   using Domain.Entities;
   using Domain.Options;
   using Entities.Interceptors;
   using Handlers;
   using Repositories;
   using Requests;
   using UnitOfWork;

   public class Read<T, TDto, TReadInterceptor, TRepository>
      : RequestHandlerFor<T, TReadInterceptor, ReadHandler<T, TDto, TReadInterceptor, TRepository>, TRepository>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadInterceptor : IReadInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public Read(Option<IUow> uow) : base(uow)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Task<Option<List<KeyValue>>> Handle(Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().HandleAsync(new ReadLookupRequest<T, TDto>(useValueAsId), ctok);
      }

      public static Task<Option<List<KeyValue>>> Handle(Option<IUow> uow, Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         var @for = new Read<T, TDto, TReadInterceptor, TRepository>(uow);
         return @for.Handle(useValueAsId, ctok);
      }

      public Task<Option<TDto>> Handle(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().HandleAsync(new ReadOneRequest<T, TDto>(criteria), ctok);
      }

      public static Task<Option<TDto>> Handle(Option<IUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Read<T, TDto, TReadInterceptor, TRepository>(uow);
         return @for.Handle(criteria, ctok);
      }

      public Task<Option<IPaged<TDto>>> Handle(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().HandleAsync(new ReadPagedRequest<T, TDto>(pageNo, pageSize, sorts, keyword), ctok);
      }

      public static Task<Option<IPaged<TDto>>> Handle(
         Option<IUow> uow,
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         var @for = new Read<T, TDto, TReadInterceptor, TRepository>(uow);
         return @for.Handle(pageNo, pageSize, sorts, keyword, ctok);
      }

      #endregion
   }
}