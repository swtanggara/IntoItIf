namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Globalization;

   internal static class TimeSpanParser
   {
      #region Public Methods and Operators

      public static string ToString(TimeSpan value)
      {
         var totalMilliseconds = (long)value.TotalMilliseconds;
         if (totalMilliseconds % 3600000L == 0L)
            return $"{totalMilliseconds / 3600000L}h";
         if (totalMilliseconds % 60000L == 0L && totalMilliseconds < 3600000L)
            return $"{totalMilliseconds / 60000L}m";
         if (totalMilliseconds % 1000L == 0L && totalMilliseconds < 60000L)
            return $"{totalMilliseconds / 1000L}s";
         if (totalMilliseconds < 1000L)
            return $"{totalMilliseconds}ms";
         return value.ToString();
      }

      public static bool TryParse(string value, out TimeSpan result)
      {
         if (!string.IsNullOrEmpty(value))
         {
            value = value.ToLowerInvariant();
            var index = value.Length - 1;
            var num = 1000;
            if (value[index] == 's')
            {
               if (value[index - 1] == 'm')
               {
                  value = value.Substring(0, value.Length - 2);
                  num = 1;
               }
               else
               {
                  value = value.Substring(0, value.Length - 1);
                  num = 1000;
               }
            }
            else if (value[index] == 'm')
            {
               value = value.Substring(0, value.Length - 1);
               num = 60000;
            }
            else if (value[index] == 'h')
            {
               value = value.Substring(0, value.Length - 1);
               num = 3600000;
            }
            else if (value.IndexOf(':') != -1)
               return TimeSpan.TryParse(value, out result);

            const NumberStyles style = NumberStyles.None;
            if (double.TryParse(value, style, CultureInfo.InvariantCulture, out var result1))
            {
               result = TimeSpan.FromMilliseconds(result1 * num);
               return true;
            }
         }

         result = new TimeSpan();
         return false;
      }

      #endregion
   }
}