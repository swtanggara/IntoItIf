namespace IntoItIf.Base.UnitOfWork
{
   using System;
   using System.Collections.Generic;
   using Domain.Options;
   using Repositories;

   internal static class UowHelper
   {
      #region Methods

      internal static Option<(Dictionary<Type, object> Repositories, TRepository Repository)> SetAndGetRepositories
         <TRepository, T>(
            this Dictionary<Type, object> repositories,
            Option<TRepository> setter)
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