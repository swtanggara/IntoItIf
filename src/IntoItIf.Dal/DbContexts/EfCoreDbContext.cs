#if NETSTANDARD2_0
namespace IntoItIf.Dal.DbContexts
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Reflection;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Helpers;
   using Helpers;
   using Microsoft.EntityFrameworkCore;

   public abstract class EfCoreDbContext : DbContext, IDbContext
   {
      #region Constructors and Destructors

      protected EfCoreDbContext()
      {
      }

      protected EfCoreDbContext(DbContextOptions options) : base(options)
      {
      }

      #endregion

      #region Public Methods and Operators

      public Option<(Expression<Func<T, bool>> Predicate, string[] PropertyNames)> BuildAlternateKeyPredicate<T>(
         T entity)
         where T : class
      {
         return EfCoreDbContextKeys.BuildAlternateKeyPredicate(this, entity);
      }

      public Option<(Expression<Func<T, bool>> Predicate, string[] PropertyNames)> BuildPrimaryKeyPredicate<T>(T entity)
         where T : class
      {
         return EfCoreDbContextKeys.BuildPrimaryKeyPredicate(this, entity);
      }

      public Option<IEnumerable<PropertyInfo>> GetAlternateKeyProperties<T>()
         where T : class
      {
         return EfCoreDbContextKeys.GetAlternateKeyProperties<T>(this).ToOption();
      }

      public Option<IEnumerable<PropertyInfo>> GetPrimaryKeyProperties<T>()
         where T : class
      {
         return EfCoreDbContextKeys.GetPrimaryKeyProperties<T>(this).ToOption();
      }

      public Option<IQueryable<T>> GetQuery<T>()
         where T : class
      {
         var typeoft = typeof(T);
         IQueryable<T> result = null;
         if (typeoft.IsAssignableTo<IEntity>()) result = Set<T>();
         else if (typeoft.IsAssignableTo<IViewEntity>()) result = Query<T>();
         return result.ToOption();
      }

      #endregion
   }
}
#endif