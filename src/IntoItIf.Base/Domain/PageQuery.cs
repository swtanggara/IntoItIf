namespace IntoItIf.Base.Domain
{
   public sealed class PageQuery : IPageQuery
   {
      #region Constants

      public const int DefaultPageSize = 10;
      public const int DefaultIndexFrom = 1;

      #endregion

      #region Constructors and Destructors

      internal PageQuery()
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

      #endregion
   }
}