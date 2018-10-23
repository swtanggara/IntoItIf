namespace IntoItIf.Base.Services.Entities.Services
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;
   using Domain.Entities;
   using Domain.Options;
   using Helpers;
   using Interceptors;
   using Repositories;
   using UnitOfWork;

   public sealed class EntityReadLookupService<T, TReadLookupInterceptor, TRepository>
      : BaseEntityService<T, TReadLookupInterceptor, TRepository>, IEntityReadLookupService
      where T : class, IEntity
      where TReadLookupInterceptor : IReadLookupInterceptor
      where TRepository : class, IRepository<T>
   {
      #region Constructors and Destructors

      public EntityReadLookupService()
      {
      }

      public EntityReadLookupService(Option<IUow> uow) : base(uow)
      {
      }

      public EntityReadLookupService(Option<IUow> uow, Option<TReadLookupInterceptor> interceptor) : base(
         uow,
         interceptor)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Option<List<KeyValue>> GetLookups(Option<bool> useValueAsId)
      {
         return ServiceMapping.GetLookups(useValueAsId);
      }

      public Task<Option<List<KeyValue>>> GetLookupsAsync(Option<bool> useValueAsId, Option<CancellationToken> ctok)
      {
         return ServiceMapping.GetLookupsAsync(useValueAsId, ctok);
      }

      #endregion
   }
}