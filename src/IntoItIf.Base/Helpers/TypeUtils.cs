namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;

   public static class TypeUtils
   {
      #region Public Methods and Operators

      public static Expression<Func<T, object>>[] GetSelectedArrayMembers<T>(params Expression<Func<T, object>>[] properties)
      {
         return properties;
      }

      public static IEnumerable<string> GetSelectedMemberNames<T>(params Expression<Func<T, object>>[] properties)
      {
         return properties.Length == 0 ? Enumerable.Empty<string>() : properties.GetMemberNames();
      }

      public static Expression<Func<T, object>>[] GetSelectedMembers<T>(params Expression<Func<T, object>>[] properties)
      {
         return properties;
      }

      #endregion
   }
}