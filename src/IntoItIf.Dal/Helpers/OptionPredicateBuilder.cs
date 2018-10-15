namespace IntoItIf.Dal.Helpers
{
   using System;
   using System.Linq.Expressions;
   using Core.Domain.Options;
   using LinqKit;

   public static class OptionPredicateBuilder
   {
      #region Public Methods and Operators

      public static Option<Expression<Func<T, bool>>> And<T>(
         this Option<Expression<Func<T, bool>>> expr1,
         Option<Expression<Func<T, bool>>> expr2)
      {
         return expr1.Combine(expr2).Map(x => x.Item1.And(x.Item2));
      }

      public static Option<Expression<Func<T, bool>>> New<T>(Option<Expression<Func<T, bool>>> expression)
      {
         return expression.Map(
            x =>
            {
               Expression<Func<T, bool>> result = PredicateBuilder.New(x);
               return result;
            });
      }

      #endregion
   }
}