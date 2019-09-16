namespace IntoItIf.Base.Domain.Results
{
   using System.Collections.Generic;
   using Domain;

   public class ResultError : ValueObject<ResultError>
   {
      public ResultError(string code, string memberInfo, string description)
      {
         Code = code;
         MemberInfo = memberInfo;
         Description = description;
      }

      #region Public Properties

      public string Code { get; }
      public string MemberInfo { get; }
      public string Description { get; }

      #endregion

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Code;
         yield return MemberInfo;
         yield return Description;
      }
   }
}