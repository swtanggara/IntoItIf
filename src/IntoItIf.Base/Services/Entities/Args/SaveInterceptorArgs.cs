namespace IntoItIf.Base.Services.Entities.Args
{
   using System;
   using System.Threading.Tasks;
   using Domain.Options;

   public class SaveInterceptorArgs<T, TDto>
   {
      #region Constructors and Destructors

      public SaveInterceptorArgs(
         Action<TDto> preValidation,
         Action<TDto> postValidation,
         Func<Option<T>, object, Task> postMap,
         Func<Option<T>, object, Task> preSave,
         Func<T, string> messageIfExistFunc,
         bool pendingSave)
      {
         PreValidation = preValidation;
         PostValidation = postValidation;
         PostMap = postMap;
         PreSave = preSave;
         MessageIfExistFunc = messageIfExistFunc;
         PendingSave = pendingSave;
      }

      #endregion

      #region Public Properties

      public Action<TDto> PreValidation { get; }
      public Action<TDto> PostValidation { get; }
      public Func<Option<T>, object, Task> PostMap { get; }
      public Func<Option<T>, object, Task> PreSave { get; }
      public Func<T, string> MessageIfExistFunc { get; }
      public bool PendingSave { get; }

      #endregion
   }
}