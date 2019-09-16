namespace IntoItIf.Base.Domain.Results
{
   using System;
   using System.Linq;
   using FluentValidation.Results;
   using Helpers;

   public static class ResultHelper
   {
      #region Constants

      public const string Runtime = "Runtime";

      #endregion

      #region Public Methods and Operators

      public static IResult ArgNotSetResult(this string source, string path, string argName)
      {
         const string thisPath = "ResultHelper.ArgNotSetResult(source, path, argName)";
         if (string.IsNullOrWhiteSpace(source))
         {
            return Result.Failed(Runtime, PredefinedResultError.ArgNotSet(thisPath, nameof(source)));
         }

         if (string.IsNullOrWhiteSpace(path))
         {
            return Result.Failed(Runtime, PredefinedResultError.ArgNotSet(thisPath, nameof(path)));
         }

         if (string.IsNullOrWhiteSpace(argName))
         {
            return Result.Failed(Runtime, PredefinedResultError.ArgNotSet(thisPath, nameof(argName)));
         }

         return Result.Failed(source, PredefinedResultError.ArgNotSet(path, argName));
      }

      public static IResult<T> ArgNotSetResult<T>(this string source, string path, string argName)
      {
         const string thisPath = "ResultHelper.ArgNotSetResult<T>(source, path, argName)";
         if (string.IsNullOrWhiteSpace(source))
         {
            return Result<T>.Failed(Runtime, PredefinedResultError.ArgNotSet(thisPath, nameof(source)));
         }

         if (string.IsNullOrWhiteSpace(path))
         {
            return Result<T>.Failed(Runtime, PredefinedResultError.ArgNotSet(thisPath, nameof(path)));
         }

         if (string.IsNullOrWhiteSpace(argName))
         {
            return Result<T>.Failed(Runtime, PredefinedResultError.ArgNotSet(thisPath, nameof(argName)));
         }

         return Result<T>.Failed(source, PredefinedResultError.ArgNotSet(path, argName));
      }

      public static IResult<IPaged<TResult>> ChangeItems<T, TResult>(
         this IResult<IPaged<T>> sourceResult,
         Func<T, TResult> selector,
         string source)
         where TResult : T
      {
         const string thisPath = "ResultHelper.ChangeItems<T, TResult>(model, selector, source)";
         if (sourceResult == null) return Runtime.ArgNotSetResult<IPaged<TResult>>(thisPath, nameof(sourceResult));
         if (!sourceResult.Succeeded) return Result<IPaged<TResult>>.Failed(thisPath, sourceResult.Errors.ToArray());
         if (selector == null) return Runtime.ArgNotSetResult<IPaged<TResult>>(thisPath, nameof(selector));
         if (string.IsNullOrWhiteSpace(source)) return Runtime.ArgNotSetResult<IPaged<TResult>>(thisPath, nameof(source));
         var paged = sourceResult.Model;
         try
         {
            return Result<IPaged<TResult>>.Success(
               new Paged<TResult>(
                  paged.Items.Select(selector),
                  ObjectDictionaryHelpers.GetPublicPropertyNames<TResult>(),
                  paged.Keys,
                  paged.TotalItemsCount,
                  paged.PageIndex,
                  paged.PageSize,
                  paged.IndexFrom,
                  true),
               source);
         }
         catch (Exception e)
         {
            var ex = new Exception(e.Message, e);
            return Result<IPaged<TResult>>.Failed(thisPath, PredefinedResultError.Exception(ex));
         }
      }

      public static IResult<TResult> ChangeModel<T, TResult>(
         this IResult<T> result,
         Func<T, TResult> selector,
         string source)
      {
         const string thisPath = "ResultHelper.ChangeModel<T, TResult>(result, selector, source)";
         if (result == null) return Runtime.ArgNotSetResult<TResult>(thisPath, nameof(result));
         if (selector == null) return Runtime.ArgNotSetResult<TResult>(thisPath, nameof(selector));
         if (string.IsNullOrWhiteSpace(source)) return Runtime.ArgNotSetResult<TResult>(thisPath, nameof(source));
         try
         {
            return Result<TResult>.Success(selector(result.Model), source);
         }
         catch (Exception e)
         {
            var ex = new Exception(e.Message, e);
            return Result<TResult>.Failed(thisPath, PredefinedResultError.Exception(ex));
         }
      }

      public static IResult<T> ToModelResult<T>(this T model, string source)
      {
         const string thisPath = "ResultHelper.ToModelResult<T>(model, source)";
         if (model == null) return Runtime.ArgNotSetResult<T>(thisPath, nameof(model));
         if (string.IsNullOrWhiteSpace(source)) return Runtime.ArgNotSetResult<T>(thisPath, nameof(source));
         return Result<T>.Success(model, source);
      }

      public static IResult ToResult(this ValidationResult validationResult, string source)
      {
         const string thisPath = "ResultHelper.ToResult(validationResult, source)";
         if (validationResult == null) return Runtime.ArgNotSetResult(thisPath, nameof(validationResult));
         if (string.IsNullOrWhiteSpace(source)) return Runtime.ArgNotSetResult(thisPath, nameof(source));
         return validationResult.IsValid
            ? Result.Success(source)
            : Result.Failed(
               source,
               validationResult.Errors?.Select(x => new ResultError(x.ErrorCode, x.PropertyName, x.ErrorMessage)).ToArray());
      }

      #endregion
   }
}