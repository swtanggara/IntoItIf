namespace IntoItIf.Core.Helpers
{
   using System;
   using System.Collections.Generic;

   internal static class ExceptionHelpers
   {
      #region Methods

      /// <summary>
      ///    Returns an array of the entire exception list in reverse order
      ///    (innermost to outermost exception)
      /// </summary>
      /// <param name="ex">The original exception to work off</param>
      /// <returns>Array of Exceptions from innermost to outermost</returns>
      internal static Exception[] GetInnerExceptions(this Exception ex)
      {
         var exceptions = new List<Exception> { ex };

         var currentEx = ex;
         while (currentEx.InnerException != null)
         {
            currentEx = currentEx.InnerException;
            exceptions.Add(currentEx);
         }

         // Reverse the order to the innermost is first
         exceptions.Reverse();

         return exceptions.ToArray();
      }

      /// <summary>
      ///    Returns the innermost Exception for an object
      /// </summary>
      /// <param name="ex"></param>
      /// <returns></returns>
      internal static Exception GetInnerMostException(this Exception ex)
      {
         var currentEx = ex;
         while (currentEx.InnerException != null)
         {
            currentEx = currentEx.InnerException;
         }

         return currentEx;
      }

      #endregion
   }
}