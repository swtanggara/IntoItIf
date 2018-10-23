namespace IntoItIf.Base.Domain
{
   using System.Collections.Generic;
   using Services;

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
      long TotalItemsCount { get; }

      #endregion
   }
}