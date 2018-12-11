namespace IntoItIf.Base.UnitOfWork
{
   using System;
   using System.Collections.Generic;
   using Repositories;

   internal static class UowHelper
   {
      #region Methods

      internal static (Dictionary<Type, object> Repositories, TRepository Repository) SetAndGetRepositories<TRepository, T>(
         this Dictionary<Type, object> repositories,
         TRepository setter)
         where TRepository : IRepository<T>
         where T : class
      {
         if (repositories == null) repositories = new Dictionary<Type, object>();

         var type = typeof(T);
         if (!repositories.ContainsKey(type))
         {
            repositories[type] = setter;
         }

         return (repositories, (TRepository)repositories[type]);
      }

      #endregion
   }
}