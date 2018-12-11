namespace IntoItIf.Base.Repositories
{
   using System;
   using System.Linq;
   using DataContexts;
   using Domain.Entities;
   using Helpers;

   internal static class RelationalRepositoryHelper<T>
      where T : class
   {
      #region Methods

      internal static bool AllKeysAreEqual(
         T existByPkEntity,
         T existByAkEntity,
         string[] pkPropertyNames)
      {
         return !pkPropertyNames.Select(
               x => new
               {
                  PropertyName = x,
                  PkValue = existByPkEntity.GetPropertyValue(x)
               })
            .Select(
               x => new
               {
                  x.PropertyName,
                  x.PkValue,
                  AkValue = existByAkEntity.GetPropertyValue(x.PropertyName)
               })
            .Where(x => x.PkValue != x.AkValue && (x.PkValue == null || !x.PkValue.Equals(x.AkValue)))
            .Select(x => x.PkValue)
            .Any();
      }

      internal static string[] GetKeyPropertyNamesInBetween(
         string[] pkPropertyNames,
         string[] akPropertyNames)
      {
         return akPropertyNames == null || !akPropertyNames.Any() ? pkPropertyNames : akPropertyNames;
      }

      internal static string[] GetSortKeys(IRelationalDataContext dbContext)
      {
         var akProperties = dbContext.GetAlternateKeyProperties<T>().ToArray();
         if (akProperties.Any()) return akProperties.Select(x => x.Name).ToArray();
         var pkProperties = dbContext.GetPrimaryKeyProperties<T>().ToArray();
         return pkProperties.Any() ? pkProperties.Select(y => y.Name).ToArray() : null;
      }

      internal static (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity)
         GiveValidatedEntityForUpdateResult(
            (T ExistByPkEntity, T ExistByAkEntity, string[] RealKeyPropertyNames, string[] PkPropertyNames, T InputEntity) parms)
      {
         var allKeysAreEqual = AllKeysAreEqual(parms.ExistByPkEntity, parms.ExistByAkEntity, parms.PkPropertyNames);
         if (parms.ExistByPkEntity == null && parms.ExistByAkEntity == null)
         {
            return ((T)null, parms.RealKeyPropertyNames, false, parms.InputEntity);
         }

         if (parms.ExistByPkEntity == null || parms.ExistByAkEntity != null && !allKeysAreEqual)
         {
            return ((T)null, parms.RealKeyPropertyNames, true, parms.InputEntity);
         }

         return (parms.ExistByPkEntity, parms.PkPropertyNames, true, parms.InputEntity);
      }

      internal static bool IfCreateError(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated)
      {
         return validated.MatchValidatedEntity != null &&
                validated.PropertyNames != null &&
                validated.PropertyNames.All(x => !string.IsNullOrWhiteSpace(x));
      }

      internal static bool IfUpdateError(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc) validated)
      {
         if (!validated.Found) return true;
         return validated.MatchValidatedEntity == null &&
                validated.PropertyNames != null &&
                validated.PropertyNames.All(x => !string.IsNullOrWhiteSpace(x));
      }

      internal static bool IsViewEntity()
      {
         return typeof(T).IsAssignableTo<IViewEntity>();
      }

      #endregion
   }
}