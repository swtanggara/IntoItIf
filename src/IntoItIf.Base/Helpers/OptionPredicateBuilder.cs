namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Linq.Expressions;
   using Domain.Options;

   internal static class OptionPredicateBuilder
   {
      #region Methods

      internal static Option<Expression<Func<T, bool>>> And<T>(
         this Option<Expression<Func<T, bool>>> expr1,
         Option<Expression<Func<T, bool>>> expr2)
      {
         return expr1.Combine(expr2)
            .Map(
               x =>
               {
                  var typeofT = typeof(T);
                  var entityParameterExpr = Expression.Parameter(typeofT, "x");
                  var body = Expression.AndAlso(x.Item1, x.Item2);
                  return Expression.Lambda<Func<T, bool>>(body, entityParameterExpr);
               });
      }

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