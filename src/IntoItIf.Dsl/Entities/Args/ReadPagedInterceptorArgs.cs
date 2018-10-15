namespace IntoItIf.Dsl.Entities.Args
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Helpers;

   public class ReadPagedInterceptorArgs<T> : ValueObject<ReadPagedInterceptorArgs<T>>
   {
      #region Constructors and Destructors

      public ReadPagedInterceptorArgs(
         string[] searchFields,
         Expression<Func<T, bool>> predicate,
         Func<object, Task> pagedValidation,
         Func<IQueryable<T>, IQueryable<T>> include,
         PageIndexFrom defaultPageIndexFrom)
      {
         SearchFields = searchFields;
         Predicate = predicate;
         PagedValidation = pagedValidation;
         Include = include;
         DefaultPageIndexFrom = defaultPageIndexFrom;
      }

      #endregion

      #region Public Properties

      public string[] SearchFields { get; }
      public Expression<Func<T, bool>> Predicate { get; }
      public Func<object, Task> PagedValidation { get; }
      public Func<IQueryable<T>, IQueryable<T>> Include { get; }
      public PageIndexFrom DefaultPageIndexFrom { get; }

      public bool IsView
      {
         get
         {
            var typeofT = typeof(T);
            return typeofT.IsAssignableTo<IViewEntity>();
         }
      }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return SearchFields;
         yield return Predicate;
         yield return PagedValidation;
         yield return Include;
         yield return DefaultPageIndexFrom;
         yield return IsView;
      }

      #endregion
   }
}