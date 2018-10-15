namespace IntoItIf.Dal.DbContexts
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Reflection;
   using Core.Domain;
   using Core.Domain.Options;

   public interface IDbContext : IDisposable, IInjectable
   {
      #region Public Methods and Operators

      Option<(Expression<Func<T, bool>> Predicate, string[] PropertyNames)> BuildAlternateKeyPredicate<T>(
         T entity)
         where T : class;

      Option<(Expression<Func<T, bool>> Predicate, string[] PropertyNames)> BuildPrimaryKeyPredicate<T>(
         T entity)
         where T : class;

      Option<IEnumerable<PropertyInfo>> GetAlternateKeyProperties<T>()
         where T : class;

      Option<IEnumerable<PropertyInfo>> GetPrimaryKeyProperties<T>()
         where T : class;

      Option<IQueryable<T>> GetQuery<T>()
         where T : class;

      #endregion
   }
}