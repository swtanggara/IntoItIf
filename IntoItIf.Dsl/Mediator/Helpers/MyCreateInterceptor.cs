namespace IntoItIf.Dsl.Mediator.Helpers
{
   using System;
   using System.Threading.Tasks;
   using Core.Domain.Options;
   using Entities.Args;
   using Entities.Interceptors;

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

   public class MyUpdateInterceptor : IUpdateInterceptor
   {
      #region Public Methods and Operators

      public SaveInterceptorArgs<T, TDto> OpenForUpdate<T, TDto>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }

   public class MyReadLookupInterceptor : IReadLookupInterceptor
   {
      #region Public Methods and Operators

      public ReadLookupInterceptorArgs<T> OpenForReadLookup<T>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }

   public class MyReadOneInterceptor : IReadOneInterceptor
   {
      #region Public Methods and Operators

      public ReadOneInterceptorArgs<T> OpenForReadOne<T>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }

   public class MyReadPagedInterceptor : IReadPagedInterceptor
   {
      #region Public Methods and Operators

      public ReadPagedInterceptorArgs<T> OpenForReadPaged<T>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }

   public class MyReadInterceptor : IReadInterceptor
   {
      #region Public Methods and Operators

      public ReadLookupInterceptorArgs<T> OpenForReadLookup<T>()
      {
         throw new NotImplementedException();
      }

      public ReadOneInterceptorArgs<T> OpenForReadOne<T>()
      {
         throw new NotImplementedException();
      }

      public ReadPagedInterceptorArgs<T> OpenForReadPaged<T>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }

   public class MyCrudInterceptor : ICrudInterceptor
   {
      #region Public Methods and Operators

      public SaveInterceptorArgs<T, TDto> OpenForCreate<T, TDto>()
      {
         throw new NotImplementedException();
      }

      public DeleteInterceptorArgs<T> OpenForDelete<T>()
      {
         throw new NotImplementedException();
      }

      public ReadLookupInterceptorArgs<T> OpenForReadLookup<T>()
      {
         throw new NotImplementedException();
      }

      public ReadOneInterceptorArgs<T> OpenForReadOne<T>()
      {
         throw new NotImplementedException();
      }

      public ReadPagedInterceptorArgs<T> OpenForReadPaged<T>()
      {
         throw new NotImplementedException();
      }

      public SaveInterceptorArgs<T, TDto> OpenForUpdate<T, TDto>()
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}