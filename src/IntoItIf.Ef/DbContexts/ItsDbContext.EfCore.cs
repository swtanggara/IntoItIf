#if NETSTANDARD2_0
namespace IntoItIf.Ef.DbContexts
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Reflection;
   using Base.DataContexts;
   using Base.Domain.Entities;
   using Base.Domain.Options;
   using Base.Helpers;
   using Helpers;
   using Microsoft.EntityFrameworkCore;

   public class ItsDbContext : DbContext, IRelationalDataContext
   {
      #region Constructors and Destructors

      protected ItsDbContext()
      {
      }

      protected ItsDbContext(DbContextOptions options) : base(options)
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
         IQueryable<T> result;
         if (typeoft.IsAssignableTo<IViewEntity>()) result = Query<T>();
         else result = Set<T>();
         return result.ToOption();
      }

      #endregion
   }
}
#endif