namespace IntoItIf.Core.Domain.Options
{
   public sealed class None
   {
      #region Constructors and Destructors

      private None()
      {
      }

      #endregion

      #region Public Properties

      public static None Value { get; } = new None();

      #endregion
   }

   public sealed class None<T> : Option<T>
   {
      #region Public Methods and Operators

      public static implicit operator T(None<T> value)
      {
         return default;
      }

      #endregion
   }
}