namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Linq.Expressions;
   using Domain.Options;

   public static class PredicateBuilder
   {
      #region Public Methods and Operators

      public static Expression<Func<T, bool>> AndAlso<T>(
         this Expression<Func<T, bool>> expr1,
         Expression<Func<T, bool>> expr2)
      {
         return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(expr1.Body, new ExpressionParameterReplacer(expr2.Parameters, expr1.Parameters).Visit(expr2.Body)),
            expr1.Parameters);
      }

      #endregion

      #region Methods

      internal static Expression<Func<T, bool>> BuildPredicate<T>(this T source)
         where T : class
      {
         return source.BuildPredicate(false);
      }

      internal static Expression<Func<T, bool>> BuildPredicate<T>(
         this T source,
         bool includeDefaultOrNull)
         where T : class
      {
         return source.BuildPredicate<T>(includeDefaultOrNull, null);
      }

      #endregion
   }
}