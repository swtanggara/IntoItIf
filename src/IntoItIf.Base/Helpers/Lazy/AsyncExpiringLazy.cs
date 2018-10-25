// From https://github.com/filipw/async-expiring-lazy

namespace IntoItIf.Base.Helpers.Lazy
{
   using System;
   using System.Threading;
   using System.Threading.Tasks;

   public class AsyncExpiringLazy<T>
   {
      #region Fields

      private readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1);
      private readonly Func<ExpiringLazyMetadata<T>, Task<ExpiringLazyMetadata<T>>> _valueProvider;
      private ExpiringLazyMetadata<T> _value;

      #endregion

      #region Constructors and Destructors

      public AsyncExpiringLazy(Func<ExpiringLazyMetadata<T>, Task<ExpiringLazyMetadata<T>>> valueProvider)
      {
         _valueProvider = valueProvider ?? throw new ArgumentNullException(nameof(valueProvider));
      }

      #endregion

      #region Properties

      private bool IsValueCreatedInternal => _value.Result != null && _value.ValidUntil > DateTimeOffset.UtcNow;

      #endregion

      #region Public Methods and Operators

      public async Task Invalidate()
      {
         await _syncLock.WaitAsync().ConfigureAwait(false);
         _value = default;
         _syncLock.Release();
      }

      public async Task<bool> IsValueCreated()
      {
         await _syncLock.WaitAsync().ConfigureAwait(false);
         try
         {
            return IsValueCreatedInternal;
         }
         finally
         {
            _syncLock.Release();
         }
      }

      public async Task<T> Value()
      {
         await _syncLock.WaitAsync().ConfigureAwait(false);
         try
         {
            if (IsValueCreatedInternal)
            {
               return _value.Result;
            }

            _value = await _valueProvider(_value).ConfigureAwait(false);
            return _value.Result;
         }
         finally
         {
            _syncLock.Release();
         }
      }

      #endregion
   }
}