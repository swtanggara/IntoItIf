namespace IntoItIf.Tests
{
   using System;
   using Dsl.Entities.Args;
   using Dsl.Entities.Interceptors;

   public class MyUpdateInterceptor : IUpdateInterceptor
   {
      #region Public Methods and Operators

      public SaveInterceptorArgs<T, TDto> OpenForUpdate<T, TDto>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}