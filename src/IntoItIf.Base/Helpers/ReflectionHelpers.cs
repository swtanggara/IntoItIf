namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Reflection;

   public static class ReflectionHelpers
   {
      #region Public Methods and Operators

      /// <summary>
      ///    Determines whether this type is assignable to <typeparamref name="T" />.
      /// </summary>
      /// <typeparam name="T">The type to test assignability to.</typeparam>
      /// <param name="this">The type to test.</param>
      /// <returns>
      ///    True if this type is assignable to references of type
      ///    <typeparamref name="T" />; otherwise, False.
      /// </returns>
      public static bool IsAssignableTo<T>(this Type @this)
      {
         if (@this == null)
            throw new ArgumentNullException(nameof(@this));
         return typeof(T).GetTypeInfo()
            .IsAssignableFrom(@this.GetTypeInfo());
      }

      public static bool IsAssignableTo(this Type @this, Type that)
      {
         if (@this == null)
            throw new ArgumentNullException(nameof(@this));
         return that.GetTypeInfo()
            .IsAssignableFrom(@this.GetTypeInfo());
      }

      #endregion
   }
}