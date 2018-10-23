namespace IntoItIf.Base.DataContexts
{
   using System;
   using System.Collections.Generic;
   using System.Linq.Expressions;
   using System.Reflection;
   using Domain.Options;

   public interface IRelationalDataContext : IDataContext
   {
      #region Public Methods and Operators

      Option<(Expression<Func<T, bool>> Predicate, string[] PropertyNames)> BuildAlternateKeyPredicate<T>(T entity)
         where T : class;

      Option<(Expression<Func<T, bool>> Predicate, string[] PropertyNames)> BuildPrimaryKeyPredicate<T>(T entity)
         where T : class;

      Option<IEnumerable<PropertyInfo>> GetAlternateKeyProperties<T>()
         where T : class;

      Option<IEnumerable<PropertyInfo>> GetPrimaryKeyProperties<T>()
         where T : class;

      #endregion
   }
}