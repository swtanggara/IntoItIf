namespace IntoItIf.Base.Domain
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   public class Paged<T> : IPaged<T>
   {
      #region Constructors and Destructors

      public Paged(
         IEnumerable<T> source,
         long totalItemsCount,
         int pageIndex = PageQuery.DefaultIndexFrom,
         int pageSize = PageQuery.DefaultPageSize,
         PageIndexFrom indexFrom = null)
      {
         var realSource = source ?? throw new ArgumentNullException(nameof(source));
         PageIndex = pageIndex;
         PageSize = pageSize;
         IndexFrom = indexFrom ?? PageIndexFrom.One;
         if (IndexFrom.Id > PageIndex)
         {
            throw new ArgumentException(
               $"indexFrom ({IndexFrom.Id}) > pageIndex ({pageIndex}), must indexFrom <= pageIndex");
         }

         TotalItemsCount = totalItemsCount;
         PagesCount = (int)Math.Ceiling(TotalItemsCount / (double)PageSize);
         Items = realSource.Skip((PageIndex - IndexFrom.Id) * PageSize)
            .Take(PageSize)
            .ToList();
      }

      #endregion

      #region Public Properties

      public bool HasNextPage => PageIndex - IndexFrom.Id + 1 < PagesCount;
      public bool HasPreviousPage => PageIndex - IndexFrom.Id > 0;
      public PageIndexFrom IndexFrom { get; }
      public IList<T> Items { get; }
      public int PageIndex { get; }
      public int PagesCount { get; }
      public int PageSize { get; }
      public long TotalItemsCount { get; }

      #endregion
   }
}