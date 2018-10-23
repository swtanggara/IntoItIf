namespace IntoItIf.Tests.Preparation
{
   using System;
   using System.Threading.Tasks;
   using Base.Domain.Options;
   using Base.Services.Entities.Args;
   using Base.Services.Entities.Interceptors;

   public class MyDeleteInterceptor : IDeleteInterceptor
   {
      #region Public Methods and Operators

      public DeleteInterceptorArgs<T> OpenForDelete<T>()
      {
         Func<Option<T>, object, Task> preDelete = null;
         Func<Option<T>, object, Task> preSave = null;
         var pendingSave = false;
         return new DeleteInterceptorArgs<T>(preDelete, preSave, pendingSave);
      }

      #endregion
   }
}