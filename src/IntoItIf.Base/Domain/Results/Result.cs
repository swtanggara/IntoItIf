namespace IntoItIf.Base.Domain.Results
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using Domain;
   using Newtonsoft.Json;

   public class Result : ValueObject<Result>, IResult
   {
      #region Constructors and Destructors

      private Result(string source, bool succeeded, IEnumerable<ResultError> errors)
      {
         Source = source ?? throw new ArgumentNullException(nameof(source));
         Succeeded = succeeded;
         Errors = errors;
      }

      #endregion

      #region Public Properties

      public string Source { get; }
      public bool Succeeded { get; }
      public IEnumerable<ResultError> Errors { get; }

      #endregion

      #region Public Methods and Operators

      public static Result Failed(string source, params ResultError[] errors)
      {
         return errors != null && errors.Length > 0
            ? new Result(source, false, errors)
            : new Result(source, false, Enumerable.Empty<ResultError>());
      }

      public static IResult FailedFrom(IResult source)
      {
         return Failed(source.Source, source.Errors.ToArray());
      }

      public static Result Success(string source)
      {
         return new Result(source, true, Enumerable.Empty<ResultError>());
      }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Source;
         yield return Succeeded;
         yield return Errors;
      }

      #endregion
   }

   public class Result<T> : ValueObject<Result>, IResult<T>
   {
      #region Constants

      private const string ThisSource = nameof(Result<T>);

      #endregion

      #region Constructors and Destructors

      private Result(T model, string source, bool succeeded, IEnumerable<ResultError> errors)
      {
         Model = model;
         Source = source ?? throw new ArgumentNullException(nameof(source));
         Succeeded = succeeded;
         Errors = errors;
      }

      private Result(IResult<T> @base) : this(@base.Model, @base.Source, @base.Succeeded, @base.Errors)
      {
      }

      #endregion

      #region Public Properties

      public T Model { get; }
      public string Source { get; }
      public bool Succeeded { get; }
      public IEnumerable<ResultError> Errors { get; }

      #endregion

      #region Public Methods and Operators

      public static Result<T> Failed(string source, params ResultError[] errors)
      {
         var thisPath = $"{nameof(Result<T>)}.Failed(source, errors)";
         if (string.IsNullOrWhiteSpace(source)) return new Result<T>(source.ArgNotSetResult<T>(thisPath, source));
         return errors != null && errors.Length > 0
            ? new Result<T>(default(T), source, false, errors)
            : new Result<T>(default(T), source, false, Enumerable.Empty<ResultError>());
      }

      public static Result<T> FailedFrom(IResult source)
      {
         return Failed(source.Source, source.Errors.ToArray());
      }

      public static Result<T> ParseFromJson(string json, string source)
      {
         try
         {
            var t = JsonConvert.DeserializeObject<T>(json);
            var result = Success(t, source);
            return result;
         }
         catch (Exception e)
         {
            var ex = new Exception(e.Message, e);
            return Failed(ThisSource, PredefinedResultError.Exception(ex));
         }
      }

      public static Result<T> Success(T model, string source)
      {
         return new Result<T>(model, source, true, Enumerable.Empty<ResultError>());
      }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Model;
         yield return Source;
         yield return Succeeded;
         yield return Errors;
      }

      #endregion

   }
}