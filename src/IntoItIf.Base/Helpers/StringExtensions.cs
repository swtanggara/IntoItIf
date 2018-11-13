namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Linq;
   using Humanizer;

   public static class StringExtensions
   {
      #region Methods

      internal static string[] Pascalize(this string[] searchFields)
      {
         if (searchFields == null || !searchFields.Any()) return searchFields;
         var result = searchFields.Select(x => x.Pascalize())
            .ToArray();
         return result;
      }

      public static string ToProperCase(this string input)
      {
         // If there are 0 or 1 characters, just return the string.
         if (input == null) return input;
         if (input.Length < 2) return input.ToUpper();

         // Start with the first character.
         string result = input.Substring(0, 1).ToUpper();

         // Add the remaining characters.
         for (int i = 1; i < input.Length; i++)
         {
            if (Char.IsUpper(input[i])) result += " ";
            result += input[i];
         }

         return result;
      }


      #endregion
   }
}