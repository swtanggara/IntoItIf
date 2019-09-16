namespace IntoItIf.Base.Domain
{
   using System.Collections.Generic;

   public class KeyValue : KeyValue<object, object>
   {
      #region Constructors and Destructors

      public KeyValue(object key, object value) : base(key, value)
      {
      }

      #endregion

      #region Public Properties


      #endregion
   }

   public class KeyValue<TKey, TValue> : ValueObject<KeyValue<TKey, TValue>>
   {
      public KeyValue(TKey key, TValue value)
      {
         Key = key;
         Value = value;
      }

      public TKey Key { get; }
      public TValue Value { get; internal set; }
      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Key;
         yield return Value;
      }
   }
}