#if NET471
namespace IntoItIf.Dal.DbContexts
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
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Helpers;

   public abstract class EfDbContext : DbContext, IDbContext
   {
      #region Constructors and Destructors

      protected EfDbContext()
      {
      }

      protected EfDbContext(DbCompiledModel model) : base(model)
      {
      }

      protected EfDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
      {
      }

      protected EfDbContext(string nameOrConnectionString, DbCompiledModel model) : base(
         nameOrConnectionString,
         model)
      {
      }

      protected EfDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
         : base(existingConnection, model, contextOwnsConnection)
      {
      }

      protected EfDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(
         existingConnection,
         contextOwnsConnection)
      {
      }

      protected EfDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(
         objectContext,
         dbContextOwnsObjectContext)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Option<(Expression<Func<T, bool>> Predicate, string[] PropertyNames)> BuildAlternateKeyPredicate<T>(
         T entity)
         where T : class
      {
         return EfDbContextKeys.BuildAlternateKeyPredicate(this, entity);
      }

      public Option<(Expression<Func<T, bool>> Predicate, string[] PropertyNames)> BuildPrimaryKeyPredicate<T>(T entity)
         where T : class
      {
         return EfDbContextKeys.BuildPrimaryKeyPredicate(this, entity);
      }

      public Option<IEnumerable<PropertyInfo>> GetAlternateKeyProperties<T>()
         where T : class
      {
         return EfDbContextKeys.GetAlternateKeyProperties<T>(this).ToOption();
      }

      public Option<IEnumerable<PropertyInfo>> GetPrimaryKeyProperties<T>()
         where T : class
      {
         return EfDbContextKeys.GetPrimaryKeyProperties<T>(this).ToOption();
      }

      public Option<IQueryable<T>> GetQuery<T>()
         where T : class
      {
         return Set<T>();
      }

      #endregion
   }
}
#endif