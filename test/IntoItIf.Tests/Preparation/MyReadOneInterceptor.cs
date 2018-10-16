namespace IntoItIf.Tests.Preparation
{
   using System;
   using Dsl.Entities.Args;
   using Dsl.Entities.Interceptors;

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