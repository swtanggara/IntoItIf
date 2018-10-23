namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Linq;
   using System.Linq.Expressions;

   internal static class ClassExtensions
   {
      #region Methods

      internal static Expression<Func<T, bool>> BuildEqualPredicateFor<T>(this T source, params string[] propertyNames)
         where T : class
      {
         var typeofT = typeof(T);
         var entityParameterExpr = Expression.Parameter(typeofT, "p");
         Expression body = null;
         foreach (var propertyName in propertyNames.GetValidatedPropertyNames<T>())
         {
            var value = source.GetPropertyValue(propertyName);
            var valueExpr = Expression.Constant(value);
            var propertyExpr = Expression.Property(entityParameterExpr, propertyName);
            var equalExpr = Expression.Equal(propertyExpr, valueExpr);
            body = body == null ? equalExpr : Expression.AndAlso(body, equalExpr);
         }

         if (body == null) return x => false;
         return Expression.Lambda<Func<T, bool>>(body, entityParameterExpr);
      }

      internal static object GetPropertyValue<T>(this T instance, string propertyName)
      {
         var typeOfT = typeof(T);
         var property = typeOfT.GetProperty(propertyName);
         var result = property?.GetValue(instance);
         return result;
      }

      internal static string[] GetValidatedPropertyNames<T>(this string[] propertyNames)
         where T : class
      {
         var tType = typeof(T);
         var result = propertyNames.Select(x => new { PropertyName = x, PropertyInfo = tType.GetProperty(x) })
            .Where(x => x.PropertyInfo != null && x.PropertyInfo.CanWrite)
            .Select(x => x.PropertyName)
            .ToArray();
         return result;
      }

      #endregion
   }
}