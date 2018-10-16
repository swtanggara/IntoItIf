namespace IntoItIf.Tests.Preparation
{
   using System;
   using Dsl.Entities.Args;
   using Dsl.Entities.Interceptors;

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