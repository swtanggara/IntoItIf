namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Linq.Expressions;
   using Domain.Options;

   public static class OptionPredicateBuilder
   {
      #region Public Methods and Operators

      public static Option<Expression<Func<T, bool>>> AndAlso<T>(
         this Option<Expression<Func<T, bool>>> expr1,
         Option<Expression<Func<T, bool>>> expr2)
      {
         return expr1.Combine(expr2)
            .Map(x => (Expr1: x.Item1, Expr2: x.Item2))
            .Map(
               x => Expression.Lambda<Func<T, bool>>(
                  Expression.AndAlso(
                     x.Expr1.Body,
                     new ExpressionParameterReplacer(x.Expr2.Parameters, x.Expr1.Parameters).Visit(x.Expr2.Body)),
                  x.Expr1.Parameters));
      }

      #endregion

      #region Methods

      internal static Option<Expression<Func<T, bool>>> BuildPredicate<T>(this Option<T> source)
         where T : class
      {
         return source.BuildPredicate(false);
      }

      internal static Option<Expression<Func<T, bool>>> BuildPredicate<T>(
         this Option<T> source,
         Option<bool> includeDefaultOrNull)
         where T : class
      {
         return source.Combine(includeDefaultOrNull, true)
            .Map(x => (Source: x.Item1, IncludeDefaultOrNull: x.Item2))
            .Map(
               x => x.Source.BuildPredicate<T>(x.IncludeDefaultOrNull));
      }

      internal static Option<Expression<Func<T, bool>>> New<T>(Option<Expression<Func<T, bool>>> expression)
      {
         return expression.Map(_ => (Expression<Func<T, bool>>)(__ => false));
      }

      #endregion
   }
}