namespace IntoItIf.Base.Services.Entities.Args
{
   using System;
   using System.Threading.Tasks;
   using Domain.Options;

   public class DeleteInterceptorArgs<T>
   {
      #region Constructors and Destructors

      public DeleteInterceptorArgs(
         Func<Option<T>, object, Task> preDelete,
         Func<Option<T>, object, Task> preSave,
         bool pendingSave)
      {
         PreDelete = preDelete;
         PreSave = preSave;
         PendingSave = pendingSave;
      }

      #endregion

      #region Public Properties

      public Func<Option<T>, object, Task> PreDelete { get; }
      public Func<Option<T>, object, Task> PreSave { get; }
      public bool PendingSave { get; }

      #endregion
   }
}