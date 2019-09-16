namespace IntoItIf.Base.Domain
{
   using System;
   using System.Collections.Generic;
   using System.Linq.Expressions;
   using Json;
   using Newtonsoft.Json;
   using Services;

   public interface IPaged<T> : IInjectable
   {
      #region Public Properties

      bool HasNextPage { get; }
      bool HasPreviousPage { get; }

      [JsonConverter(typeof(EnumerationJsonConverter<PageIndexFrom>))]
      PageIndexFrom IndexFrom { get; }

      IList<T> Items { get; }

      [JsonConverter(typeof(DictionaryStringPairsAsArrayOfKeyValuePairsConverter))]
      IDictionary<string, string> Fields { get; }

      IEnumerable<string> Keys { get; }

      int PageIndex { get; }
      int PagesCount { get; }
      int PageSize { get; }
      long TotalItemsCount { get; }

      #endregion

      #region Public Methods and Operators

      IEnumerable<string> GetIgnoredFields();
      void IgnoreFields(params Expression<Func<T, object>>[] properties);
      void OverrideDisplays(params (Expression<Func<T, object>> Prop, string Display)[] overrides);
      void OnlyShowFields(params Expression<Func<T, object>>[] toIndex);

      #endregion
   }
}