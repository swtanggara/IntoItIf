#if NET471
namespace IntoItIf.Dal.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Data.Entity;
   using System.Data.Entity.Core.Metadata.Edm;
   using System.Data.Entity.Infrastructure;
   using System.Data.Entity.Infrastructure.Annotations;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Reflection;

   internal static class EfDbContextKeys
   {
      #region Methods

      internal static (Expression<Func<T, bool>> Predicate, string[] PropertyNames) BuildAlternateKeyPredicate<T>(
         this DbContext source,
         T entity)
         where T : class
      {
         var akProperties = source.GetIndexedProperties<T>(true).Select(x => x.Name).ToArray();
         var result = entity.BuildEqualPredicateFor(akProperties);
         return (result, akProperties);
      }

      internal static (Expression<Func<T, bool>> Predicate, string[] PropertyNames) BuildPrimaryKeyPredicate<T>(
         this DbContext source,
         T entity)
         where T : class
      {
         var pkProperties = source.GetPrimaryKeyProperties<T>().Select(x => x.Name).ToArray();
         var result = entity.BuildEqualPredicateFor(pkProperties);
         return (result, pkProperties);
      }

      internal static IEnumerable<PropertyInfo> GetAlternateKeyProperties<T>(this DbContext dbContext)
      {
         var result = dbContext.GetIndexedProperties(typeof(T), true);
         return result;
      }

      internal static IEnumerable<PropertyInfo> GetPrimaryKeyProperties<T>(this DbContext dbContext)
         where T : class
      {
         var result = dbContext.GetPrimaryKeyProperties(typeof(T));
         return result;
      }

      private static IEnumerable<PropertyInfo> GetIndexedProperties<T>(
         this DbContext dbContext,
         bool uniqueIndexOnly = false)
         where T : class
      {
         var result = dbContext.GetIndexedProperties(typeof(T), uniqueIndexOnly);
         return result;
      }

      private static IEnumerable<PropertyInfo> GetIndexedProperties(
         this DbContext dbContext,
         Type type,
         bool uniqueIndexOnly = false)
      {
         var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
         var storageEntityType = objectContext.MetadataWorkspace
            .GetItems(DataSpace.SSpace)
            .Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType)
            .OfType<EntityType>()
            .Single(x => x.Name == type.Name);
         Func<MetadataProperty, bool> plainIndex = x => x.Value is IndexAnnotation;
         Func<MetadataProperty, bool> uniqueIndex = x =>
         {
            return x.Value is IndexAnnotation indexAnnotation && indexAnnotation.Indexes.Any(x1 => x1.IsUnique);
         };
         var metadataPropertiesPredicate = uniqueIndexOnly ? uniqueIndex : plainIndex;

         var result = storageEntityType.Properties
            .Where(x => x.MetadataProperties.Any(metadataPropertiesPredicate))
            .Select(x => type.GetProperty(x.Name))
            .ToList();
         return result;
      }

      private static IEnumerable<PropertyInfo> GetPrimaryKeyProperties(this DbContext dbContext, Type type)
      {
         var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
         var entityType = objectContext.MetadataWorkspace
            .GetItems(DataSpace.OSpace)
            .Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType)
            .OfType<EntityType>()
            .Single(x => x.Name == type.Name);
         return entityType.KeyMembers
            .Select(x => type.GetProperty(x.Name))
            .ToList();
      }

      #endregion
   }
}
#endif