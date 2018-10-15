namespace IntoItIf.Dsl.Entities.Services
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;
   using Helpers;
   using Interceptors;

   public sealed class EntityReadLookupService<T, TReadLookupInterceptor>
      : BaseEntityService<T, TReadLookupInterceptor>, IEntityReadLookupService
      where T : class, IEntity
      where TReadLookupInterceptor : IReadLookupInterceptor
   {
      #region Constructors and Destructors

      public EntityReadLookupService()
      {
      }

      public EntityReadLookupService(Option<BaseUnitOfWork> uow) : base(uow)
      {
      }

      public EntityReadLookupService(Option<BaseUnitOfWork> uow, Option<TReadLookupInterceptor> interceptor) : base(
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