namespace IntoItIf.Tests.Preparation
{
   using System;
   using Base.Services.Entities.Args;
   using Base.Services.Entities.Interceptors;

   public class MyReadOneInterceptor : IReadOneInterceptor
   {
      #region Public Methods and Operators

      public ReadOneInterceptorArgs<T> OpenForReadOne<T>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}