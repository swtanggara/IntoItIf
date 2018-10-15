namespace IntoItIf.Core.Domain
{
   public class KeyValue
   {
      #region Constructors and Destructors

      public KeyValue(object key, object value)
      {
         Key = key;
         Value = value;
      }

      #endregion

      #region Public Properties

      public object Key { get; }
      public object Value { get; }

      #endregion
   }
}