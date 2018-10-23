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

   public class ReadLookup<T, TDto, TReadLookupInterceptor, TRepository>
      : RequestHandlerFor<T, TReadLookupInterceptor, ReadLookupHandler<T, TDto, TReadLookupInterceptor, TRepository>, TRepository>
      where T : class, IEntity
      where TDto : class, IDto
      where TReadLookupInterceptor : IReadLookupInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public ReadLookup(Option<IUow> uow) : base(uow)
      {
      }

      #endregion

      #region Public Methods and Operators

      public static Task<Option<List<KeyValue>>> Handle(Option<IUow> uow, Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         var @for = new ReadLookup<T, TDto, TReadLookupInterceptor, TRepository>(uow);
         return @for.Handle(useValueAsId, ctok);
      }

      public Task<Option<List<KeyValue>>> Handle(Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         return Handler().ReduceOrDefault().Handle(new ReadLookupRequest<T, TDto>(useValueAsId), ctok);
      }

      #endregion
   }
}