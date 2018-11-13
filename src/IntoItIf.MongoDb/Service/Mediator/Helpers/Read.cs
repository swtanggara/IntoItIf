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

   public class Read<T, TDto, TReadInterceptor> : Read<T, TDto, TReadInterceptor, MongoRepository<T>>
      where T : class, IMongoEntity
      where TDto : class, IDto
      where TReadInterceptor : IReadInterceptor
   {
      #region Constructors and Destructors

      public Read(Option<MongoUow> uow) : base(uow.ReduceOrDefault())
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<List<KeyValue>>> Handle(Option<MongoUow> uow, Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         var @for = new Read<T, TDto, TReadInterceptor>(uow);
         return @for.Handle(useValueAsId, ctok);
      }

      public static Task<Option<TDto>> Handle(Option<MongoUow> uow, Option<TDto> criteria, Option<CancellationToken> ctok)
      {
         var @for = new Read<T, TDto, TReadInterceptor>(uow);
         return @for.Handle(criteria, ctok);
      }

      public static Task<Option<IPaged<TDto>>> Handle(
         Option<MongoUow> uow,
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok)
      {
         var @for = new Read<T, TDto, TReadInterceptor>(uow);
         return @for.Handle(pageNo, pageSize, sorts, keyword, ctok);
      }

      #endregion
   }
}