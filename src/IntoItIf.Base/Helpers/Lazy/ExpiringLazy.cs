// From https://github.com/filipw/async-expiring-lazy

namespace IntoItIf.Base.Helpers.Lazy
{
   using System;

   public class ExpiringLazy<T>
   {
      #region Fields

      private readonly Func<ExpiringLazyMetadata<T>, ExpiringLazyMetadata<T>> _valueProvider;
      private ExpiringLazyMetadata<T> _value;

      #endregion

      #region Constructors and Destructors

      public ExpiringLazy(Func<ExpiringLazyMetadata<T>, ExpiringLazyMetadata<T>> valueProvider)
      {
         _valueProvider = valueProvider ?? throw new ArgumentNullException(nameof(valueProvider));
      }

      #endregion

      #region Properties

      private bool IsValueCreatedInternal => _value.Result != null && _value.ValidUntil > DateTimeOffset.UtcNow;

      #endregion

      #region Public Methods and Operators

      public void Invalidate()
      {
         _value = default;
      }

      public bool IsValueCreated()
      {
         return IsValueCreatedInternal;
      }

      public T Value()
      {
         if (IsValueCreatedInternal)
         {
            return _value.Result;
      }

         _value = _valueProvider(_value);
         return _value.Result;
      }

      #endregion
   }
}