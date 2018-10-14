namespace IntoItIf.Core.Domain.Options
{
   public sealed class Some<T> : Option<T>
   {
      #region Constructors and Destructors

      public Some(T content)
      {
         Content = content;
      }

      #endregion

      #region Public Properties

      public T Content { get; }

      #endregion

      #region Public Methods and Operators

      public static implicit operator T(Some<T> value)
      {
         return value.Content;
      }

      #endregion
   }
}