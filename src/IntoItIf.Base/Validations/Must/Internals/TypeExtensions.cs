namespace IntoItIf.Base.Validations.Must.Internals
{
   using System;

   internal static class TypeExtensions
   {
      public static bool IsValueType(this Type type)
      {
         return type.IsValueType;
      }

      public static bool IsGenericType(this Type type)
      {
         return type.IsGenericType;
      }
   }
}