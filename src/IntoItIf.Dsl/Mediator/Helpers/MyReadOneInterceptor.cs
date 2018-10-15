namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System;
   using Entities.Args;
   using Entities.Interceptors;

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