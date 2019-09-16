namespace IntoItIf.AspNetCore.Identity
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Reflection;
   using Base.Domain.Entities;
   using Base.Helpers;
   using Ef.DbContexts;
   using Ef.Helpers;
   using Microsoft.AspNetCore.Identity;
   using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
   using Microsoft.EntityFrameworkCore;

   public abstract class ItsIdDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
      : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IItsDbContext
      where TUser : IdentityUser<TKey>
      where TRole : IdentityRole<TKey>
      where TKey : IEquatable<TKey>
      where TUserClaim : IdentityUserClaim<TKey>
      where TUserRole : IdentityUserRole<TKey>
      where TUserLogin : IdentityUserLogin<TKey>
      where TRoleClaim : IdentityRoleClaim<TKey>
      where TUserToken : IdentityUserToken<TKey>
   {
      #region Constructors and Destructors

      protected ItsIdDbContext(DbContextOptions options) : base(options)
      {
      }

      protected ItsIdDbContext()
      {
      }

      #endregion

      #region Public Methods and Operators

      public (Expression<Func<T, bool>> Predicate, string[] PropertyNames) BuildAlternateKeyPredicate<T>(T entity)
         where T : class
      {
         return EfCoreDbContextKeys.BuildAlternateKeyPredicate(this, entity);
      }

      public (Expression<Func<T, bool>> Predicate, string[] PropertyNames) BuildPrimaryKeyPredicate<T>(T entity)
         where T : class
      {
         return EfCoreDbContextKeys.BuildPrimaryKeyPredicate(this, entity);
      }

      public IEnumerable<PropertyInfo> GetAlternateKeyProperties<T>()
         where T : class
      {
         return EfCoreDbContextKeys.GetAlternateKeyProperties<T>(this);
      }

      public IEnumerable<PropertyInfo> GetPrimaryKeyProperties<T>()
         where T : class
      {
         return EfCoreDbContextKeys.GetPrimaryKeyProperties<T>(this);
      }

      public IQueryable<T> GetQuery<T>()
         where T : class
      {
         var typeoft = typeof(T);
         IQueryable<T> result;
         if (typeoft.IsAssignableTo<IViewEntity>()) result = Query<T>();
         else result = Set<T>();
         return result;
      }

      #endregion
   }
}