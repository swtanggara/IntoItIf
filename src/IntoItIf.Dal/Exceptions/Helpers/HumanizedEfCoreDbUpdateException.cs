#if NETSTANDARD2_0
namespace IntoItIf.Dal.Exceptions.Helpers
{
   using System.Data.SqlClient;
   using Core.Helpers;
   using Microsoft.EntityFrameworkCore;

   internal static class HumanizedEfCoreDbUpdateException
   {
      #region Methods

      internal static string HumanizeDbError(this DbUpdateException ex)
      {
         var result = ex.GetBaseException()
            .Message;
         if (result.StartsWith("The DELETE statement conflicted with the REFERENCE constraint"))
         {
            result = "Unable to delete data specified. It is used by another data";
            return result;
         }

         var exInnerMost = ex.GetInnerMostException();
         result = exInnerMost.Message;
         if (exInnerMost is SqlException sqlException)
         {
            switch (sqlException.Number)
            {
               case 547:
                  result = sqlException.HumanizeFkConstraintError();
                  return result;
               case 2601:
                  result = sqlException.HumanizeUxConstraintError();
                  return result;
            }
         }

         return result;
      }

      internal static string HumanizeDeleteError(this DbUpdateException ex)
      {
         var result = ex.GetBaseException()
            .Message;
         if (result.StartsWith("The DELETE statement conflicted with the REFERENCE constraint"))
         {
            result = "Unable to delete data specified. It is used by another data";
         }

         return result;
      }

      internal static string HumanizeKeyError(this DbUpdateException ex)
      {
         var exInnerMost = ex.GetInnerMostException();
         var result = exInnerMost.Message;
         if (exInnerMost is SqlException sqlException)
         {
            switch (sqlException.Number)
            {
               case 547:
                  result = sqlException.HumanizeFkConstraintError();
                  break;
               case 2601:
                  result = sqlException.HumanizeUxConstraintError();
                  break;
            }
         }

         return result;
      }

      #endregion
   }
}
#endif