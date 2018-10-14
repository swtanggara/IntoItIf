namespace IntoItIf.Dsl.Entities.Args
{
   using System;
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Options;

   public class DeleteInterceptorArgs<T> : ValueObject<DeleteInterceptorArgs<T>>
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

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return PreDelete;
         yield return PreSave;
         yield return PendingSave;
      }

      #endregion
   }
}