namespace IntoItIf.MongoDb.Service.Mediator.Helpers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Services.Entities.Interceptors;
   using Base.Services.Mediator.Helpers;

   public class Crud<T, TDto, TCrudInterceptor> : Crud<T, TDto, TCrudInterceptor, MongoRepository<T>>
      where T : class, IMongoEntity
      where TDto : class, IDto
      where TCrudInterceptor : ICrudInterceptor
   {
      #region Constructors and Destructors

      public Crud(Option<MongoUow> uow) : base(uow.ReduceOrDefault())
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<Dictionary<string, object>>> HandleCreate(
         Option<MongoUow> uow,
         Option<TDto> dto,
         Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>(uow);
         return @for.HandleCreate(dto, ctok);
      }

      public static Task<Option<bool>> HandleDelete(Option<MongoUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>(uow);
         return @for.HandleDelete(criteria, ctok);
      }

      public static Task<Option<List<KeyValue>>> HandleReadLookup(
         Option<MongoUow> uow,
         Option<bool> useValueAsId,
         Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>(uow);
         return @for.HandleReadLookup(useValueAsId, ctok);
      }

      public static Task<Option<TDto>> HandleReadOne(Option<MongoUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>(uow);
         return @for.HandleReadOne(criteria, ctok);
      }

      public static Task<Option<IPaged<TDto>>> HandleReadPaged(
         Option<MongoUow> uow,
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>(uow);
         return @for.HandleReadPaged(pageNo, pageSize, sorts, keyword, ctok);
      }

      public static Task<Option<Dictionary<string, object>>> HandleUpdate(
         Option<MongoUow> uow,
         Option<TDto> dto,
         Option<CancellationToken> ctok)
      {
         var @for = new Crud<T, TDto, TCrudInterceptor>(uow);
         return @for.HandleUpdate(dto, ctok);
      }

      #endregion
   }
}