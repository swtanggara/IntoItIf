namespace IntoItIf.Base.DataContexts
{
   using System;
   using System.Collections.Generic;
   using System.Linq.Expressions;
   using System.Reflection;

   public interface IRelationalDataContext : IDataContext
   {
      #region Public Methods and Operators

      (Expression<Func<T, bool>> Predicate, string[] PropertyNames) BuildAlternateKeyPredicate<T>(T entity)
         where T : class;

      (Expression<Func<T, bool>> Predicate, string[] PropertyNames) BuildPrimaryKeyPredicate<T>(T entity)
         where T : class;

      IEnumerable<PropertyInfo> GetAlternateKeyProperties<T>()
         where T : class;

      IEnumerable<PropertyInfo> GetPrimaryKeyProperties<T>()
         where T : class;

      #endregion
   }
}