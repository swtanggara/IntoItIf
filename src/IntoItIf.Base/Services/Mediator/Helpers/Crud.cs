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

   public class Crud<T, TDto, TCrudInterceptor, TRepository>
      : RequestHandlerFor<T, TCrudInterceptor, CrudHandler<T, TDto, TCrudInterceptor, TRepository>, TRepository>
      where T : class, IEntity
      where TDto : class, IDto
      where TCrudInterceptor : ICrudInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public Crud(Option<IUow> uow) : base(uow)
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<Dictionary<string, object>>> HandleCreate(
         Option<IUow> uow,
         Option<TDto> dto,
         Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor, TRepository>(uow);
         return @for.HandleCreate(dto, ctok);
      }

      public static Task<Option<bool>> HandleDelete(Option<IUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor, TRepository>(uow);
         return @for.HandleDelete(criteria, ctok);
      }

      public static Task<Option<List<KeyValue>>> HandleReadLookup(
         Option<IUow> uow,
         Option<bool> useValueAsId,
         Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor, TRepository>(uow);
         return @for.HandleReadLookup(useValueAsId, ctok);
      }

      public static Task<Option<TDto>> HandleReadOne(Option<IUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor, TRepository>(uow);
         return @for.HandleReadOne(criteria, ctok);
      }

      public static Task<Option<IPaged<TDto>>> HandleReadPaged(
         Option<IUow> uow,
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor, TRepository>(uow);
         return @for.HandleReadPaged(pageNo, pageSize, sorts, keyword, ctok);
      }

      public static Task<Option<Dictionary<string, object>>> HandleUpdate(
         Option<IUow> uow,
         Option<TDto> dto,
         Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor, TRepository>(uow);
         return @for.HandleUpdate(dto, ctok);
      }

      public Task<Option<Dictionary<string, object>>> HandleCreate(Option<TDto> dto, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().Handle(new CreateRequest<T, TDto>(dto), ctok);
      }

      public Task<Option<bool>> HandleDelete(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().Handle(new DeleteRequest<T, TDto>(criteria), ctok);
      }

      public Task<Option<List<KeyValue>>> HandleReadLookup(Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().Handle(new ReadLookupRequest<T, TDto>(useValueAsId), ctok);
      }

      public Task<Option<TDto>> HandleReadOne(Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().Handle(new ReadOneRequest<T, TDto>(criteria), ctok);
      }

      public Task<Option<IPaged<TDto>>> HandleReadPaged(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().Handle(new ReadPagedRequest<T, TDto>(pageNo, pageSize, sorts, keyword), ctok);
      }

      public Task<Option<Dictionary<string, object>>> HandleUpdate(Option<TDto> dto, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().Handle(new UpdateRequest<T, TDto>(dto), ctok);
      }

      #endregion
   }
}