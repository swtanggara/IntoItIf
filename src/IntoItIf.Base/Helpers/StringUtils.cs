namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using System.Text;

   public static class StringUtils
   {
      #region Constants

      public const char CarriageReturn = '\r';
      public const string CarriageReturnLineFeed = "\r\n";
      public const string Empty = "";
      public const char LineFeed = '\n';
      public const char Tab = '\t';

      #endregion

      #region Enums

      internal enum SnakeCaseState
      {
         Start,
         Lower,
         Upper,
         NewWord,
      }

      #endregion

      #region Public Methods and Operators

      public static StringWriter CreateStringWriter(int capacity)
      {
         return new StringWriter(new StringBuilder(capacity), CultureInfo.InvariantCulture);
      }

      public static bool EndsWith(this string source, char value)
      {
         if (source.Length > 0)
            return source[source.Length - 1] == value;
         return false;
      }

      public static TSource ForgivingCaseSensitiveFind<TSource>(
         this IEnumerable<TSource> source,
         Func<TSource, string> valueSelector,
         string testValue)
      {
         if (source == null)
            throw new ArgumentNullException(nameof(source));
         if (valueSelector == null)
            throw new ArgumentNullException(nameof(valueSelector));
         var source1 = source.Where(
            s => string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase));
         if (source1.Count() <= 1)
            return source1.SingleOrDefault();
         return source
            .SingleOrDefault(s => string.Equals(valueSelector(s), testValue, StringComparison.Ordinal));
      }

      public static string FormatWith(this string format, IFormatProvider provider, object arg0)
      {
         return FormatWith(format, provider, new[] { arg0 });
      }

      public static string FormatWith(
         this string format,
         IFormatProvider provider,
         object arg0,
         object arg1)
      {
         return FormatWith(format, provider, new[] { arg0, arg1 });
      }

      public static string FormatWith(
         this string format,
         IFormatProvider provider,
         object arg0,
         object arg1,
         object arg2)
      {
         return FormatWith(format, provider, new[] { arg0, arg1, arg2 });
      }

      public static string FormatWith(
         this string format,
         IFormatProvider provider,
         object arg0,
         object arg1,
         object arg2,
         object arg3)
      {
         return FormatWith(format, provider, new[] { arg0, arg1, arg2, arg3 });
      }

      public static bool IsHighSurrogate(char c)
      {
         return char.IsHighSurrogate(c);
      }

      public static bool IsLowSurrogate(char c)
      {
         return char.IsLowSurrogate(c);
      }

      /// <summary>
      ///    Determines whether the string is all white space. Empty string will return <c>false</c>.
      /// </summary>
      /// <param name="s">The string to test whether it is all white space.</param>
      /// <returns>
      ///    <c>true</c> if the string is all white space; otherwise, <c>false</c>.
      /// </returns>
      public static bool IsWhiteSpace(string s)
      {
         if (s == null)
            throw new ArgumentNullException(nameof(s));
         if (s.Length == 0)
            return false;
         for (var index = 0; index < s.Length; ++index)
         {
            if (!char.IsWhiteSpace(s[index]))
               return false;
         }

         return true;
      }

      public static bool StartsWith(this string source, char value)
      {
         if (source.Length > 0)
            return source[0] == value;
         return false;
      }

      public static string ToCamelCase(string s)
      {
         if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            return s;
         var charArray = s.ToCharArray();
         for (var index = 0; index < charArray.Length && (index != 1 || char.IsUpper(charArray[index])); ++index)
         {
            var flag = index + 1 < charArray.Length;
            if ((index > 0) & flag && !char.IsUpper(charArray[index + 1]))
            {
               if (char.IsSeparator(charArray[index + 1]))
               {
                  charArray[index] = ToLower(charArray[index]);
               }

               break;
            }

            charArray[index] = ToLower(charArray[index]);
         }

         return new string(charArray);
      }

      public static void ToCharAsUnicode(char c, char[] buffer)
      {
         buffer[0] = '\\';
         buffer[1] = 'u';
         buffer[2] = MathUtils.IntToHex((c >> 12) & 15);
         buffer[3] = MathUtils.IntToHex((c >> 8) & 15);
         buffer[4] = MathUtils.IntToHex((c >> 4) & 15);
         buffer[5] = MathUtils.IntToHex(c & 15);
      }

      public static string ToLowerTrueFalse(this bool source)
      {
         return source ? "true" : "false";
      }

      public static string ToLowerYesNo(this bool source)
      {
         return source ? "yes" : "no";
      }

      public static string ToSnakeCase(string s)
      {
         if (string.IsNullOrEmpty(s))
            return s;
         var stringBuilder = new StringBuilder();
         var snakeCaseState = SnakeCaseState.Start;
         for (var index = 0; index < s.Length; ++index)
         {
            if (s[index] == ' ')
            {
               if (snakeCaseState != SnakeCaseState.Start)
                  snakeCaseState = SnakeCaseState.NewWord;
            }
            else if (char.IsUpper(s[index]))
            {
               switch (snakeCaseState)
               {
                  case SnakeCaseState.Lower:
                  case SnakeCaseState.NewWord:
                     stringBuilder.Append('_');
                     break;
                  case SnakeCaseState.Upper:
                     var flag = index + 1 < s.Length;
                     if ((index > 0) & flag)
                     {
                        var c = s[index + 1];
                        if (!char.IsUpper(c) && c != '_')
                        {
                           stringBuilder.Append('_');
                        }
                     }

                     break;
               }

               var lower = char.ToLower(s[index], CultureInfo.InvariantCulture);
               stringBuilder.Append(lower);
               snakeCaseState = SnakeCaseState.Upper;
            }
            else if (s[index] == '_')
            {
               stringBuilder.Append('_');
               snakeCaseState = SnakeCaseState.Start;
            }
            else
            {
               if (snakeCaseState == SnakeCaseState.NewWord)
                  stringBuilder.Append('_');
               stringBuilder.Append(s[index]);
               snakeCaseState = SnakeCaseState.Lower;
            }
         }

         return stringBuilder.ToString();
      }

      public static string ToTrueFalse(this bool source)
      {
         return source ? "True" : "False";
      }

      public static string ToYesNo(this bool source)
      {
         return source ? "Yes" : "No";
      }

      public static string Trim(this string s, int start, int length)
      {
         if (s == null)
            throw new ArgumentNullException();
         if (start < 0)
            throw new ArgumentOutOfRangeException(nameof(start));
         if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));
         var index = start + length - 1;
         if (index >= s.Length)
            throw new ArgumentOutOfRangeException(nameof(length));
         while (start < index && char.IsWhiteSpace(s[start]))
            ++start;
         while (index >= start && char.IsWhiteSpace(s[index]))
            --index;
         return s.Substring(start, index - start + 1);
      }

      #endregion

      #region Methods

      private static string FormatWith(
         this string format,
         IFormatProvider provider,
         params object[] args)
      {
         format = Ensure.IsNotNull(format, nameof(format));
         return string.Format(provider, format, args);
      }

      private static char ToLower(char c)
      {
         c = char.ToLower(c, CultureInfo.InvariantCulture);
         return c;
      }

      #endregion
   }
}