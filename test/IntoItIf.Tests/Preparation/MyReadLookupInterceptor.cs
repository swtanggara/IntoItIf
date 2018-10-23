namespace IntoItIf.Tests.Preparation
{
   using System;
   using Base.Services.Entities.Args;
   using Base.Services.Entities.Interceptors;

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