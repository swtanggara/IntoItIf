namespace IntoItIf.Dal.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using Core.Domain.Options;
   using Core.Helpers;
   using Humanizer;
   using LinqKit;

   internal static class ObjectDictionaryHelpers
   {
      #region Methods

      internal static Expression<Func<T, bool>> BuildPredicate<T>(
         this IDictionary<string, object> source,
         bool includeDefaultOrNull = false)
         where T : class
      {
         var typeofT = typeof(T);
         var predicate = PredicateBuilder.New<T>();
         var entityParameterExpr = Expression.Parameter(typeofT, "p");
         var pascalizedPropertyNames = source.Keys.Select(x => x.Pascalize())
            .ToArray();
         var validatedPropertyNames = pascalizedPropertyNames.GetValidatedPropertyNames<T>();
         foreach (var propertyName in validatedPropertyNames)
         {
            var propInfo = typeofT.GetProperty(propertyName);
            if (propInfo != null)
            {
               var propDefaultValue = propInfo.PropertyType.IsValueType
                  ? Activator.CreateInstance(propInfo.PropertyType)
                  : null;
               var value = source[propertyName];
               if (propDefaultValue == null && value == null) continue;
               if (value.Equals(propDefaultValue) && !includeDefaultOrNull) continue;
               var propExpr = Expression.Property(entityParameterExpr, propertyName);
               var valueExpr = Expression.Constant(value);
               var equalExpr = Expression.Equal(propExpr, valueExpr);
               var lambdaExpr = Expression.Lambda<Func<T, bool>>(equalExpr, entityParameterExpr);
               predicate = predicate.And(lambdaExpr);
            }
         }

         return predicate;
      }

      internal static Expression<Func<T, bool>> BuildPredicate<T>(
         this object source,
         bool includeDefaultOrNull = false)
         where T : class
      {
         var queryDictionary = source.ToDictionary();
         var result = queryDictionary.BuildPredicate<T>(includeDefaultOrNull);
         return result;
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

      #endregion
   }
}