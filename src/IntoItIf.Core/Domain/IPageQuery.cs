namespace IntoItIf.Core.Domain
{
   public interface IPageQuery : IInjectable
   {
      #region Public Properties

      string[] SearchFields { get; }
      int PageIndex { get; }
      int PageSize { get; }
      string Keyword { get; }
      string[] Sorts { get; }
      PageIndexFrom IndexFrom { get; }

      #endregion
   }
}