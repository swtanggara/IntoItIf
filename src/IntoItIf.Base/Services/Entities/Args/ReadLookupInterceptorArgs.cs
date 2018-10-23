namespace IntoItIf.Base.Services.Entities.Args
{
   using System;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading.Tasks;
   using Base.Helpers;
   using Domain.Entities;

   public class ReadLookupInterceptorArgs<T>
   {
      #region Constructors and Destructors

      public ReadLookupInterceptorArgs(
         string idProperty,
         string valueProperty,
         Expression<Func<T, bool>> predicate,
         Func<object, Task> lookupValidation,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         IdProperty = idProperty;
         ValueProperty = valueProperty;
         Predicate = predicate;
         LookupValidation = lookupValidation;
         Include = include;
      }

      #endregion

      #region Public Properties

      public string IdProperty { get; }
      public string ValueProperty { get; }
      public Expression<Func<T, bool>> Predicate { get; }
      public Func<object, Task> LookupValidation { get; }
      public Func<IQueryable<T>, IQueryable<T>> Include { get; }

      public bool IsView
      {
         get
         {
            var typeofT = typeof(T);
            return typeofT.IsAssignableTo<IViewEntity>();
         }
      }

      #endregion
   }
}