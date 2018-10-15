namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System;
   using Entities.Args;
   using Entities.Interceptors;

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