namespace IntoItIf.Dsl.Entities.Services
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Options;

   public interface IEntityReadLookupService
   {
      #region Public Methods and Operators

      Option<List<KeyValue>> GetLookups(Option<bool> useValueAsId);
      Task<Option<List<KeyValue>>> GetLookupsAsync(Option<bool> useValueAsId, Option<CancellationToken> ctok);

      #endregion
   }
}