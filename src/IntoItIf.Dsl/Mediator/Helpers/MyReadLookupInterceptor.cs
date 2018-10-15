namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System;
   using Entities.Args;
   using Entities.Interceptors;

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