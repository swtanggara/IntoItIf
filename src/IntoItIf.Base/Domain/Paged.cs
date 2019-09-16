namespace IntoItIf.Base.Domain
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using Helpers;

   public class Paged<T> : IPaged<T>
   {
      #region Fields

      private List<string> _ignoredFields = new List<string>();

      #endregion

      #region Constructors and Destructors

      public Paged(
         IEnumerable<T> source,
         long totalItemsCount,
         int pageIndex = PageQuery.DefaultIndexFrom,
         int pageSize = PageQuery.DefaultPageSize,
         PageIndexFrom indexFrom = null,
         bool alreadyPaged = false) : this(source, null, null, totalItemsCount, pageIndex, pageSize, indexFrom, alreadyPaged)
      {
      }

      public Paged(
         IEnumerable<T> source,
         IDictionary<string, string> fields,
         IEnumerable<string> keys,
         long totalItemsCount,
         int pageIndex = PageQuery.DefaultIndexFrom,
         int pageSize = PageQuery.DefaultPageSize,
         PageIndexFrom indexFrom = null,
         bool alreadyPaged = false)
      {
         var realSource = source ?? throw new ArgumentNullException(nameof(source));
         PageIndex = pageIndex;
         PageSize = pageSize;
         IndexFrom = indexFrom ?? PageIndexFrom.One;
         if (IndexFrom.Id > PageIndex && PageIndex != 0)
         {
            throw new ArgumentException(
               $"indexFrom ({IndexFrom.Id}) > pageIndex ({pageIndex}), must indexFrom <= pageIndex");
         }

         TotalItemsCount = totalItemsCount;
         PagesCount = (int)Math.Ceiling(TotalItemsCount / (double)PageSize);
         if (PageIndex > PagesCount) PageIndex = PagesCount;
         if (alreadyPaged)
         {
            Items = realSource.ToList();
         }
         else
         {
            Items = realSource.Skip((PageIndex - IndexFrom.Id) * PageSize)
               .Take(PageSize)
               .ToList();
         }

         Fields = fields;
         Keys = keys;
      }

      #endregion

      #region Public Properties

      public IDictionary<string, string> Fields { get; private set; }
      public IEnumerable<string> Keys { get; }
      public bool HasNextPage => PageIndex - IndexFrom.Id + 1 < PagesCount;
      public bool HasPreviousPage => PageIndex - IndexFrom.Id > 0;

      public PageIndexFrom IndexFrom { get; }

      public IList<T> Items { get; }
      public int PageIndex { get; }
      public int PagesCount { get; }
      public int PageSize { get; }
      public long TotalItemsCount { get; }

      #endregion

      #region Public Methods and Operators

      public IEnumerable<string> GetIgnoredFields()
      {
         return _ignoredFields;
      }

      public void IgnoreFields(params Expression<Func<T, object>>[] properties)
      {
         _ignoredFields = new List<string>();
         var memberNames = properties.GetMemberNames();
         foreach (var memberName in memberNames)
         {
            if (Fields.ContainsKey(memberName))
            {
               Fields.Remove(memberName);
               _ignoredFields.Add(memberName);
            }
         }
      }

      public void OnlyShowFields(params Expression<Func<T, object>>[] overrides)
      {
         var newFields = new Dictionary<string, string>();
         foreach (var @override in overrides)
         {
            var memberName = @override.GetMemberName();
            if (Fields.ContainsKey(memberName))
            {
               newFields.Add(memberName, Fields[memberName]);
            }
         }

         _ignoredFields = new List<string>(Fields.Where(x => !newFields.Select(y => y.Key).Contains(x.Key)).Select(x => x.Key));
         Fields = newFields;
      }

      public void OverrideDisplays(params (Expression<Func<T, object>> Prop, string Display)[] overrides)
      {
         foreach (var @override in overrides)
         {
            var memberName = @override.Prop.GetMemberName();
            if (Fields.ContainsKey(memberName))
            {
               Fields[memberName] = @override.Display;
            }
         }
      }

      #endregion
   }
}