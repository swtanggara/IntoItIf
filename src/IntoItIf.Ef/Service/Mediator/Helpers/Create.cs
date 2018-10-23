namespace IntoItIf.Ef.Service.Mediator.Helpers
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Services.Entities.Interceptors;
   using Base.Services.Mediator.Helpers;
   using Repositories;
   using UnitOfWork;

   public class Create<T, TDto, TCreateInterceptor> : Create<T, TDto, TCreateInterceptor, EfRepository<T>>
      where T : class, IEntity
      where TDto : class, IDto
      where TCreateInterceptor : ICreateInterceptor
   {
      #region Constructors and Destructors

      public Create(Option<EfUow> uow) : base(uow.ReduceOrDefault())
      {
      }

      #endregion

      public static Task<Option<Dictionary<string, object>>> Handle(Option<EfUow> uow, Option<TDto> dto, Option<CancellationToken> ctok)
      {
         var @for = new Create<T, TDto, TCreateInterceptor>(uow);
         return @for.Handle(dto, ctok);
      }

   }
}