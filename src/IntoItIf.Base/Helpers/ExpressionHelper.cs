namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;

   public static class ExpressionHelper
   {
      private const string ExpressionCannotBeNullMessage = "The expression cannot be null.";
      private const string InvalidExpressionMessage = "Invalid expression.";

      public static string GetMemberName<T>(this Expression<Func<T, object>> expression)
      {
         return GetMemberName(expression.Body);
      }

      public static List<string> GetMemberNames<T>(this IEnumerable<Expression<Func<T, object>>> expressions)
      {
         return expressions.Select(x => GetMemberName(x.Body)).ToList();
      }

      public static string GetMemberName<T>(this Expression<Action<T>> expression)
      {
         return GetMemberName(expression.Body);
      }

      private static string GetMemberName(Expression expression)
      {
         switch (expression)
         {
            case null:
               throw new ArgumentException(ExpressionCannotBeNullMessage);
            case MemberExpression memberExpression:
               // Reference type property or field
               return memberExpression.Member.Name;
            case MethodCallExpression methodCallExpression:
               // Reference type method
               return methodCallExpression.Method.Name;
            case UnaryExpression unaryExpression:
               // Property, field of method returning value type
               return GetMemberName(unaryExpression);
            default:
               throw new ArgumentException(InvalidExpressionMessage);
         }
      }

      private static string GetMemberName(UnaryExpression unaryExpression)
      {
         if (!(unaryExpression.Operand is MethodCallExpression)) return ((MemberExpression)unaryExpression.Operand).Member.Name;
         var methodExpression = (MethodCallExpression)unaryExpression.Operand;
         return methodExpression.Method.Name;

      }
   }
}