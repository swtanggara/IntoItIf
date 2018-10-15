#if NETSTANDARD2_0
namespace IntoItIf.Dal.Exceptions.Helpers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Options;
   using Microsoft.EntityFrameworkCore;

   internal static class HumanizedEfCoreDbUpdateConcurrencyException
   {
      #region Methods

      internal static string HumanizeUpdateError(
         this DbUpdateConcurrencyException ex)
      {
         var entry = ex.Entries.Single();
         var dbValues = entry.GetDatabaseValues();
         var result = dbValues == null
            ? "Unable to save changes. Data was deleted by another user."
            : "Unable to save changes. The data you attempted to update was modified by another user after you " +
              "opened it. This update will be canceled. Please open this data again to get updated RowVersion, " +
              "then try again later.";
         return result;
      }

      internal static async Task<string> HumanizeUpdateErrorAsync(
         this DbUpdateConcurrencyException ex,
         Option<CancellationToken> ctok)
      {
         var entry = ex.Entries.Single();
         var dbValues = await entry.GetDatabaseValuesAsync(ctok.ReduceOrDefault());
         var result = dbValues == null
            ? "Unable to save changes. Data was deleted by another user."
            : "Unable to save changes. The data you attempted to update was modified by another user after you " +
              "opened it. This update will be canceled. Please open this data again to get updated RowVersion, " +
              "then try again later.";
         return result;
      }

      #endregion
   }
}
#endif