namespace IntoItIf.Base.Domain
{
   using Options;

   public sealed class PageQuery : IPageQuery
   {
      #region Constants

      public const int DefaultPageSize = 10;

      #endregion

      #region Static Fields

      public static readonly PageIndexFrom DefaultIndexFrom = PageIndexFrom.One;

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

      public static Option<PageQuery> Get(
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom)
      {
         return Get(pageIndex, pageSize, sorts, keyword, indexFrom, None.Value);
      }

      #endregion

      #region Methods

      internal static Option<PageQuery> Get(
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<string[]> searchFields)
      {
         return pageIndex.Combine(pageSize, true, DefaultPageSize)
            .Combine(sorts, true)
            .Combine(keyword, true)
            .Combine(indexFrom, true, DefaultIndexFrom)
            .Combine(searchFields, true)
            .Map(
               x => new PageQuery
               {
                  PageIndex = x.Item1.Item1.Item1.Item1.Item1,
                  PageSize = x.Item1.Item1.Item1.Item1.Item2,
                  Sorts = x.Item1.Item1.Item1.Item2,
                  Keyword = x.Item1.Item1.Item2,
                  IndexFrom = x.Item1.Item2,
                  SearchFields = x.Item2
               });
      }

      #endregion
   }
}