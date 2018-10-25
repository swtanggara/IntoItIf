// From https://github.com/filipw/async-expiring-lazy

namespace IntoItIf.Base.Helpers.Lazy
{
   using System;

   public struct ExpiringLazyMetadata<T>
   {
      public T Result { get; set; }

      public DateTimeOffset ValidUntil { get; set; }
   }
}