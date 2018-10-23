namespace IntoItIf.Base.Domain.Options
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
         return None.Value;
      }

#pragma warning disable RCS1163 // Unused parameter.
      public static implicit operator Option<T>(None none)
#pragma warning restore RCS1163 // Unused parameter.
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