namespace IntoItIf.Dal.Helpers
{
   using System;
   using System.Linq.Expressions;
   using Core.Helpers;
   using LinqKit;

   internal static class ClassExtensions
   {
      #region Methods

      internal static Expression<Func<T, bool>> BuildEqualPredicateFor<T>(this T source, params string[] propertyNames)
         where T : class
      {
         var predicate = PredicateBuilder.New<T>();
         var entityParameterExpr = Expression.Parameter(typeof(T), "p");
         var validatedPropertyNames = propertyNames.GetValidatedPropertyNames<T>();
         foreach (var propertyName in validatedPropertyNames)
         {
            var value = source.GetPropertyValue(propertyName);
            var valueExpr = Expression.Constant(value);
            var propertyExpr = Expression.Property(entityParameterExpr, propertyName);
            var equalExpr = Expression.Equal(propertyExpr, valueExpr);
            var lambdExpr = Expression.Lambda<Func<T, bool>>(equalExpr, entityParameterExpr);
            predicate = predicate.And(lambdExpr);
         }

         return predicate;
      }

      #endregion
   }
}