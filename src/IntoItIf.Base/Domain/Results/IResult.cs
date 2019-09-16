namespace IntoItIf.Base.Domain.Results
{
   using System.Collections.Generic;

   public interface IResult
   {
      #region Public Properties

      string Source { get; }
      bool Succeeded { get; }
      IEnumerable<ResultError> Errors { get; }

      #endregion
   }

   public interface IResult<out T> : IResult
   {
      T Model { get; }
   }
}