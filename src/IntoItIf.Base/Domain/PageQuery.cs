namespace IntoItIf.Base.Domain
{
   using System;
   using System.Linq;
   using System.Linq.Expressions;
   using Helpers;

   public sealed class PageQuery : IPageQuery
   {
      #region Constants

      public const int MaxPageSize = 500;
      public const int DefaultPageSize = 10;
      public const int DefaultIndexFrom = 1;

      #endregion

      #region Constructors and Destructors

      private PageQuery()
      {
      }

      #endregion

      #region Public Properties

      public string[] SearchFields { get; private set; }
      public int PageIndex { get; private set; }
      public int PageSize { get; private set; }
      public string Keyword { get; private set; }
      public string[] Sorts { get; private set; }
      public PageIndexFrom IndexFrom { get; private set; }

      #endregion

      #region Public Methods and Operators

      public static PageQuery Get(
         int pageIndex = DefaultIndexFrom,
         int pageSize = DefaultPageSize,
         string[] sorts = null,
         string keyword = null,
         PageIndexFrom indexFrom = null,
         string[] searchFields = null)
      {
         if (pageSize < 1 || pageSize > MaxPageSize)
         {
            throw new ArgumentOutOfRangeException(
               nameof(pageSize),
               $"Please validate your pageSize. It must be greater than 0 and not exceed {MaxPageSize}");
         }
         return new PageQuery
         {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Sorts = sorts,
            Keyword = keyword,
            IndexFrom = indexFrom ?? PageIndexFrom.One,
            SearchFields = searchFields
         };
      }

      public static string[] DefineSearchFields<T>(
         params Expression<Func<T, object>>[] properties)
      {
         return TypeUtils.GetSelectedMemberNames(properties).ToArray();
      }

      #endregion
   }
}