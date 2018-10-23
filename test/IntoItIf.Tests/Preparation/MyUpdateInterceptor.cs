namespace IntoItIf.Tests.Preparation
{
   using System;
   using Base.Services.Entities.Args;
   using Base.Services.Entities.Interceptors;

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