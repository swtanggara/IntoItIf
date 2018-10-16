namespace IntoItIf.Tests.Preparation
{
   using System;
   using System.Threading.Tasks;
   using Core.Domain.Options;
   using Dsl.Entities.Args;
   using Dsl.Entities.Interceptors;

   public class MyCreateInterceptor : ICreateInterceptor
   {
      #region Public Methods and Operators

      public SaveInterceptorArgs<T, TDto> OpenForCreate<T, TDto>()
      {
         Action<TDto> preValidation = null;
         Action<TDto> postValidation = null;
         Func<Option<T>, object, Task> postMap = null;
         Func<Option<T>, object, Task> preSave = null;
         Func<T, string> messageIfExistFunc = null;
         var pendingSave = false;
         return new SaveInterceptorArgs<T, TDto>(
            preValidation,
            postValidation,
            postMap,
            preSave,
            messageIfExistFunc,
            pendingSave);
      }

      #endregion
   }
}