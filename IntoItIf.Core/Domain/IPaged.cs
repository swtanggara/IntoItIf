namespace IntoItIf.Core.Domain
{
   using System.Collections.Generic;

   public interface IPaged<T> : IInjectable
   {
      #region Public Properties

      bool HasNextPage { get; }
      bool HasPreviousPage { get; }
      PageIndexFrom IndexFrom { get; }
      IList<T> Items { get; }
      int PageIndex { get; }
      int PagesCount { get; }
      int PageSize { get; }
      int TotalItemsCount { get; }

      #endregion
   }
}