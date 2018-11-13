namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Linq.Expressions;
   using Domain.Options;

   public static class PredicateBuilder
   {
      public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
      {
         return OptionPredicateBuilder.AndAlso<T>(expr1, expr2).ReduceOrDefault();
      }
   }
}