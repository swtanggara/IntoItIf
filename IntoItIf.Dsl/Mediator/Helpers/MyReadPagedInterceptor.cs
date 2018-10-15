namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System;
   using Entities.Args;
   using Entities.Interceptors;

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