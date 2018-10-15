namespace IntoItIf.Core.Domain.Options
{
   public abstract class Bool<T> : Option<T>
   {
      #region Constructors and Destructors

      internal Bool(Option<T> output, bool value)
      {
         Output = output;
         Value = value;
      }

      #endregion

      #region Public Properties

      public Option<T> Output { get; }
      public bool Value { get; }

      #endregion
   }

   public sealed class Bool<TIn, TOut> : Bool<TOut>
   {
      #region Constructors and Destructors

      private Bool(Option<TIn> input, Option<TOut> output, bool value) : base(output, value)
      {
         Input = input;
      }

      #endregion

      #region Public Properties

      public Option<TIn> Input { get; }

      #endregion

      #region Public Methods and Operators

      public static Bool<TIn, TOut> False(Option<TIn> input)
      {
         return new Bool<TIn, TOut>(input, null, false);
      }

      public static Bool<TIn, TOut> True(Option<TIn> input, Option<TOut> output)
      {
         return new Bool<TIn, TOut>(input, output, true);
      }

      public bool IsFalse()
      {
         return !IsTrue();
      }

      public bool IsTrue()
      {
         return Input.IsSome() && Output.IsSome() && Value;
      }

      #endregion
   }
}