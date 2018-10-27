namespace IntoItIf.Base.Repositories
{
   using System;
   using System.Linq;
   using DataContexts;
   using Domain.Entities;
   using Domain.Options;
   using Helpers;

   internal static class RelationalRepositoryHelper<T>
      where T : class
   {
      #region Methods

      internal static Option<bool> AllKeysAreEqual(
         Option<T> existByPkEntity,
         Option<T> existByAkEntity,
         Option<string[]> pkPropertyNames)
      {
         return pkPropertyNames.Combine(existByPkEntity)
            .Combine(existByAkEntity)
            .Map(
               x =>
               {
                  var y = (
                     PkPropertyNames: x.Item1.Item1,
                     ExistByPkEntity: x.Item1.Item2,
                     ExistByAkEntity: x.Item2
                  );
                  foreach (var propertyName in y.PkPropertyNames)
                  {
                     var pkValue = y.ExistByPkEntity.GetPropertyValue(propertyName);
                     var akValue = y.ExistByAkEntity.GetPropertyValue(propertyName);
                     if (pkValue != akValue && (pkValue == null || !pkValue.Equals(akValue))) return false;
                  }

                  return true;
               });
      }

      internal static Option<string[]> GetKeyPropertyNamesInBetween(
         Option<string[]> pkPropertyNames,
         Option<string[]> akPropertyNames)
      {
         return pkPropertyNames.Combine(akPropertyNames)
            .Map(
               x =>
               {
                  var y = (PkPropertyNames: x.Item1, AkPropertyNames: x.Item2);
                  return y.AkPropertyNames == null || !y.AkPropertyNames.Any() ? y.PkPropertyNames : y.AkPropertyNames;
               });
      }

      internal static Option<string[]> GetSortKeys(Option<IRelationalDataContext> dbContext)
      {
         return dbContext.Map(
               x => (DbContext: x, AkProperties: x.GetAlternateKeyProperties<T>().ReduceOrDefault()))
            .IfMap(
               x => x.AkProperties.Any(),
               x => x.AkProperties.Select(y => y.Name).ToArray())
            .ElseMap(
               x =>
               {
                  var pkProperties = x.DbContext.GetPrimaryKeyProperties<T>().ReduceOrDefault().ToArray();
                  return pkProperties.Any() ? pkProperties.Select(y => y.Name).ToArray() : null;
               })
            .Output;
      }

      internal static Option<(T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity)>
         GiveValidatedEntityForUpdateResult(
            Option<(
               T ExistByPkEntity,
               T ExistByAkEntity,
               string[] RealKeyPropertyNames,
               string[] PkPropertyNames,
               T InputEntity)> parms)
      {
         return parms.Map(
               y => (
                  y.ExistByPkEntity,
                  y.ExistByAkEntity,
                  y.RealKeyPropertyNames,
                  y.PkPropertyNames,
                  AllKeysAreEqual: AllKeysAreEqual(y.ExistByPkEntity, y.ExistByAkEntity, y.PkPropertyNames)
                     .ReduceOrDefault(),
                  y.InputEntity
               ))
            .IfMap(
               y => y.ExistByPkEntity == null && y.ExistByAkEntity == null,
               y => (
                  MatchValidatedEntity: (T)null,
                  PropertyNames: y.RealKeyPropertyNames,
                  Found: false,
                  y.InputEntity
               ))
            .ElseMap(
               y =>
               {
                  if (y.ExistByPkEntity == null || y.ExistByAkEntity != null && y.AllKeysAreEqual)
                     return (
                        MatchValidatedEntity: (T)null,
                        PropertyNames: y.RealKeyPropertyNames,
                        Found: true,
                        y.InputEntity
                     );

                  return (
                     MatchValidatedEntity: y.ExistByPkEntity,
                     PropertyNames: y.PkPropertyNames,
                     Found: true,
                     y.InputEntity
                  );
               })
            .Output;
      }

      internal static bool IfCreateError(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated)
      {
         var result = validated.MatchValidatedEntity != null &&
                validated.PropertyNames != null &&
                validated.PropertyNames.All(x => !string.IsNullOrWhiteSpace(x));
         return result;
      }

      internal static bool IfUpdateError(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc) validated)
      {
         if (!validated.Found) return true;
         return validated.MatchValidatedEntity == null &&
                validated.PropertyNames != null &&
                validated.PropertyNames.All(x => !string.IsNullOrWhiteSpace(x));
      }

      internal static Option<bool> IsViewEntity()
      {
         return typeof(T).IsAssignableTo<IViewEntity>();
      }

      #endregion
   }
}