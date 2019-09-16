namespace IntoItIf.Base.Validations.Must
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Runtime.CompilerServices;

   public class MustBeException : Exception
   {
      #region Constructors and Destructors

      internal MustBeException(string message)
      {
         Message = message;
      }

      internal MustBeException(string methodName, object actual, object expected, Type parentType, string propertyName, Type propertyType)
      {
         Message = CreateMessage(methodName, actual, expected, parentType, propertyName, propertyType);
      }

      #endregion

      #region Public Properties

      public override string Message { get; }

      #endregion

      #region Methods

      private static string CreateMessage(
         string methodName,
         object actual,
         object expected,
         Type parentType,
         string propertyName,
         Type propertyType)
      {
         string GetName()
         {
            if (parentType == propertyType)
            {
               return propertyName.Replace("_", "");
            }

            return $"{parentType.Name}.{propertyName}";
         }

         string GetValues()
         {
            var enumerableExpected = (IEnumerable)expected;
            var result = new List<string>();
            foreach (var item in enumerableExpected)
            {
               result.Add($"{item}");
            }

            return string.Join(", ", result);
         }
         
         Enum.TryParse<MustBeMethod>(methodName, out var mustBeMethod);
         switch (mustBeMethod)
         {
            case MustBeMethod.MustBe:
               return $"{GetName()} must be {expected}. Given value: {actual}";
            case MustBeMethod.MustNotBe:
               return $"{GetName()} must not be {expected}. Given value: {actual}";

            case MustBeMethod.MustBeNull:
               return $"{GetName()} must be null. Given value: {actual}";
            case MustBeMethod.MustNotBeNull:
               return $"{GetName()} must not be null. Given value: {actual}";

            case MustBeMethod.MustBeOneOf:
               return $"{GetName()} must be one of ({GetValues()}). Given value: {actual}";
            case MustBeMethod.MustNotBeOneOf:
               return $"{GetName()} must not be one of ({GetValues()}). Given value: {actual}";

            case MustBeMethod.MustBeNullOrEmpty:
               return $"{GetName()} must be null or empty. Given value: {actual}";
            case MustBeMethod.MustBeNullOrWhiteSpace:
               return $"{GetName()} must be null or whitespace. Given value: {actual}";
            case MustBeMethod.MustNotBeNullOrEmpty:
               return $"{GetName()} must not be null or empty. Given value: {actual}";
            case MustBeMethod.MustNotBeNullOrWhiteSpace:
               return $"{GetName()} must not be null or whitespace. Given value: {actual}";

            case MustBeMethod.MustBePositive:
               return $"{GetName()} must be positive. Given value: {actual}";
            case MustBeMethod.MustBeNegative:
               return $"{GetName()} must be negative. Given value: {actual}";

            case MustBeMethod.MustBeInRange:
               return $"{GetName()} must be in range {expected}. Given value: {actual}";

            default:
               throw new ArgumentOutOfRangeException();
         }
      }

      #endregion
   }

   public class MustBeException<T, TProp> : MustBeException
   {
      #region Constructors and Destructors

      internal MustBeException(
         PropertyMustBe<T, TProp> mustBe,
         object expected,
         [CallerMemberName] string mustBeMethod = null) : base(
         mustBeMethod,
         mustBe.Value,
         expected,
         typeof(T),
         mustBe.Name,
         typeof(TProp))
      {
      }

      #endregion
   }
}