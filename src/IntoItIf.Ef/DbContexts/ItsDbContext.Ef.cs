#if NET471
namespace IntoItIf.Ef.DbContexts
{
   using System;
   using System.Collections.Generic;
   using System.Data.Common;
   using System.Data.Entity;
   using System.Data.Entity.Core.Objects;
   using System.Data.Entity.Infrastructure;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Reflection;
   using Base.DataContexts;
   using Helpers;

   public class ItsDbContext : DbContext, IRelationalDataContext
   {
      #region Constructors and Destructors

      protected ItsDbContext()
      {
      }

      protected ItsDbContext(DbCompiledModel model) : base(model)
      {
      }

      protected ItsDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
      {
      }

      protected ItsDbContext(string nameOrConnectionString, DbCompiledModel model) : base(
         nameOrConnectionString,
         model)
      {
      }

      protected ItsDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
         : base(existingConnection, model, contextOwnsConnection)
      {
      }

      protected ItsDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(
         existingConnection,
         contextOwnsConnection)
      {
      }

      protected ItsDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(
         objectContext,
         dbContextOwnsObjectContext)
      {
      }

      #endregion

      #region Public Methods and Operators

      public (Expression<Func<T, bool>> Predicate, string[] PropertyNames) BuildAlternateKeyPredicate<T>(
         T entity)
         where T : class
      {
         return EfDbContextKeys.BuildAlternateKeyPredicate(this, entity);
      }

      public (Expression<Func<T, bool>> Predicate, string[] PropertyNames) BuildPrimaryKeyPredicate<T>(T entity)
         where T : class
      {
         return EfDbContextKeys.BuildPrimaryKeyPredicate(this, entity);
      }

      public IEnumerable<PropertyInfo> GetAlternateKeyProperties<T>()
         where T : class
      {
         return EfDbContextKeys.GetAlternateKeyProperties<T>(this);
      }

      public IEnumerable<PropertyInfo> GetPrimaryKeyProperties<T>()
         where T : class
      {
         return EfDbContextKeys.GetPrimaryKeyProperties<T>(this);
      }

      public IQueryable<T> GetQuery<T>()
         where T : class
      {
         return Set<T>();
      }

      #endregion
   }
}
#endif