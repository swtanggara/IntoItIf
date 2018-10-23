namespace IntoItIf.Tests.Preparation
{
   using System;
   using Base.Services.Entities.Args;
   using Base.Services.Entities.Interceptors;

   public class MyReadPagedInterceptor : IReadPagedInterceptor
   {
      #region Public Methods and Operators

      public ReadPagedInterceptorArgs<T> OpenForReadPaged<T>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}