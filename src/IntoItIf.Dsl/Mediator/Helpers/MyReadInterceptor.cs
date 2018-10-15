namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System;
   using Entities.Args;
   using Entities.Interceptors;

   public class MyReadInterceptor : IReadInterceptor
   {
      #region Public Methods and Operators

      public ReadLookupInterceptorArgs<T> OpenForReadLookup<T>()
      {
         throw new NotImplementedException();
      }

      public ReadOneInterceptorArgs<T> OpenForReadOne<T>()
      {
         throw new NotImplementedException();
      }

      public ReadPagedInterceptorArgs<T> OpenForReadPaged<T>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}