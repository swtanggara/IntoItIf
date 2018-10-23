namespace IntoItIf.Base.Helpers
{
   using System.Linq;
   using Humanizer;

   internal static class StringExtensions
   {
      #region Methods

      internal static string[] Pascalize(this string[] searchFields)
      {
         if (searchFields == null || !searchFields.Any()) return searchFields;
         var result = searchFields.Select(x => x.Pascalize())
            .ToArray();
         return result;
      }

      #endregion
   }
}