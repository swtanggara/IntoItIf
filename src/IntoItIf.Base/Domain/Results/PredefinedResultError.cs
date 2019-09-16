namespace IntoItIf.Base.Domain.Results
{
   using System;

   public class PredefinedResultError : ResultError
   {
      #region Constants

      private const string ThisSource = nameof(PredefinedResultError);

      #endregion

      #region Constructors and Destructors

      private PredefinedResultError(string code, string memberInfo, string description) : base(code, memberInfo, description)
      {
      }

      #endregion

      #region Public Methods and Operators

      public static PredefinedResultError ArgNotSet(string path, string arg)
      {
         if (path == null)
         {
            return new PredefinedResultError(
               "-001",
               "PredefinedResultError.ArgNotSet argument path",
               "As you unexpected, the path you forgot to set");
         }

         if (arg == null)
         {
            return new PredefinedResultError(
               "-001",
               "PredefinedResultError.ArgNotSet argument arg",
               "As you unexpected, the arg you forgot to set");
         }

         var memberInfo = $"{path} argument {arg}";
         return new PredefinedResultError("-001", memberInfo, $"As you unexpected, the {memberInfo} you forgot to set");
      }

      public static PredefinedResultError Exception(Exception ex)
      {
         var thisPath = $"{ThisSource}.Exception(ex)";
         if (ex == null) return ArgNotSet(thisPath, nameof(ex));
         return new PredefinedResultError($"{ex.HResult}", nameof(ex), ex.Message);
      }

      public static PredefinedResultError InvalidId(string idName)
      {
         var thisPath = $"{ThisSource}.InvalidId(idName)";
         if (string.IsNullOrWhiteSpace(idName)) return ArgNotSet(thisPath, nameof(idName));
         return new PredefinedResultError("011", nameof(idName), "Invalid keys");
      }

      public static PredefinedResultError InvalidKeys(string argName)
      {
         var thisPath = $"{ThisSource}.InvalidKeys(argName)";
         if (string.IsNullOrWhiteSpace(argName)) return ArgNotSet(thisPath, nameof(argName));
         return new PredefinedResultError("010", nameof(argName), "Invalid keys");
      }

      public static PredefinedResultError JustError(string message)
      {
         var thisPath = $"{ThisSource}.JustError(message)";
         if (string.IsNullOrWhiteSpace(message)) return ArgNotSet(thisPath, nameof(message));
         return new PredefinedResultError("012", nameof(message), message);
      }

      public static PredefinedResultError NotFound(string message)
      {
         var thisPath = $"{ThisSource}.NotFound(message)";
         if (string.IsNullOrWhiteSpace(message)) return ArgNotSet(thisPath, nameof(message));
         return new PredefinedResultError("090", nameof(message), message);
      }

      public static PredefinedResultError ObjectNotSet(string memberInfo)
      {
         if (memberInfo == null)
         {
            return new PredefinedResultError(
               "-000",
               "PredefinedResultError.ObjectNotSet args memberInfo",
               "As you unexpected, the memberInfo you forgot to set");
         }

         return new PredefinedResultError("-000", memberInfo, $"As you unexpected, the {memberInfo} you forgot to set");
      }

      #endregion
   }
}