namespace IntoItIf.Ef.Service.Mediator.Helpers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Services.Entities.Interceptors;
   using Base.Services.Mediator.Helpers;
   using Repositories;
   using UnitOfWork;

   public class ReadLookup<T, TDto, TReadLookupInterceptor> : ReadLookup<T, TDto, TReadLookupInterceptor, EfRepository<T>>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadLookupInterceptor : IReadLookupInterceptor
   {
      #region Constructors and Destructors

      public ReadLookup(Option<EfUow> uow) : base(uow.ReduceOrDefault())
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<List<KeyValue>>> Handle(Option<EfUow> uow, Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         var @for = new ReadLookup<T, TDto, TReadLookupInterceptor>(uow);
         return @for.Handle(useValueAsId, ctok);
      }

      #endregion
   }
}