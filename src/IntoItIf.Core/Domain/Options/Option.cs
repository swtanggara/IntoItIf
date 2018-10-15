namespace IntoItIf.Core.Domain.Options
{
   using System;

   public abstract class Option<T>
   {
      #region Constructors and Destructors

      internal Option()
      {
      }

      #endregion

      #region Public Methods and Operators

      public static implicit operator Option<T>(T value)
      {
         if (value != null) return new Some<T>(value);
         Option<T> result = None.Value;
         return result;
      }

      public static implicit operator Option<T>(None none)
      {
         return new None<T>();
      }

      public static implicit operator Option<T>(Exception exception)
      {
         return Fail<T>.Throw(exception);
      }

      public bool IsFail()
      {
         return this is Fail<T>;
      }

      public bool IsNone()
      {
         return this is None<T>;
      }

      public bool IsSome()
      {
         return this is Some<T>;
      }

      #endregion
   }
}