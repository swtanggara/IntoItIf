namespace IntoItIf.Dal.Exceptions.Helpers
{
   using System;
   using System.Data.SqlClient;
   using System.Linq;

   internal static class HumanizedSqlException
   {
      #region Methods

      internal static string HumanizeFkConstraintError(this SqlException ex)
      {
         var msg = ex.Message;
         var result = msg;
         const string startsWith = "The INSERT statement conflicted with the FOREIGN KEY constraint";
         if (msg.StartsWith(startsWith))
         {
            var splittedMsgs = msg.Split(new[] { ". " }, StringSplitOptions.RemoveEmptyEntries);
            if (splittedMsgs.Length == 2)
            {
               var firstMsg = splittedMsgs.First();
               var splittedFirstSentences = firstMsg.Split(new[] { "\"" }, StringSplitOptions.RemoveEmptyEntries);
               if (splittedFirstSentences.Length == 2)
               {
                  var fkName = splittedFirstSentences.Last();
                  var fkNameParts = fkName.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                  if (fkNameParts.Length == 3)
                  {
                     var parent = fkNameParts.Last();
                     var parentParts = parent.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                     if (parentParts.Length == 2)
                     {
                        var fkColumnName = parentParts.Last();
                        result = $"Invalid \'{fkColumnName}\' value";
                     }
                  }
               }
            }
         }

         return result;
      }

      internal static string HumanizeUxConstraintError(this SqlException ex)
      {
         var msg = ex.Message;
         var result = msg;
         var keywords = new[]
         {
            "Cannot insert duplicate key row in object",
            "with unique index",
            "The duplicate key value is"
         };
         if (msg.Contains(keywords[0]) && msg.Contains(keywords[1]) && msg.Contains(keywords[2]))
         {
            var splittedMsgs = msg.Split(new[] { ". ", ".\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (splittedMsgs.Length == 3)
            {
               var firstMsg = splittedMsgs.First();
               var splittedFirstSentences = firstMsg.Split(new[] { "\'" }, StringSplitOptions.RemoveEmptyEntries);
               if (splittedFirstSentences.Length > 0)
               {
                  var uxName = splittedFirstSentences.Last();
                  var uxNameParts = uxName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                  if (uxNameParts.Length > 0)
                  {
                     var uxColumnName = uxNameParts.Last();
                     var secondMsg = splittedMsgs[1];
                     var uxColumnValue = secondMsg.Replace($"{keywords[2]} ", "")
                        .Replace("(", "")
                        .Replace(")", "");
                     result = $"{uxColumnName} \'{uxColumnValue}\' already used by another data";
                  }
               }
            }
         }

         return result;
      }

      #endregion
   }
}