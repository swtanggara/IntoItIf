namespace IntoItIf.Tests
{
   using System;
   using Dsl.Entities.Args;
   using Dsl.Entities.Interceptors;

   public class MyReadLookupInterceptor : IReadLookupInterceptor
   {
      #region Public Methods and Operators

      public ReadLookupInterceptorArgs<T> OpenForReadLookup<T>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}