namespace IntoItIf.Base.Validations.Must
{
   using System;
   using System.Linq.Expressions;
   using System.Reflection;

   public static class MustBe
   {
      #region Public Methods and Operators

      public static PropertyMustBe<T, TProp> For<T, TProp>(this T @this, Expression<Func<T, TProp>> selector)
      {
         if (selector.Body is MemberExpression memberExpr)
         {
            var member = memberExpr.Member;
            if (member is PropertyInfo prop)
            {
               return new PropertyMustBe<T, TProp>(@this, prop.Name, (TProp)prop.GetValue(@this));
            }
         }

         throw new MustBeException("Must.For<T, TProp>() selector must be a property selector.");
      }

      public static PropertyMustBe<T, T> ForSelf<T>(this T @this, Expression<Func<T, T>> selfSelector)
      {
         if (selfSelector.Body is ParameterExpression paramExpr)
         {
            return new PropertyMustBe<T, T>(@this, paramExpr.Name, @this);
         }

         throw new MustBeException("Must.ForSelf<T>() selfSelector must be a self selector.");
      }

      public static PropertyMustBe<T, TProp> SwitchTo<TOld, T, TProp>(
         this PropertyMustBe<TOld, T> @this,
         Expression<Func<T, TProp>> nextSelector)
      {
         var member = ((MemberExpression)nextSelector.Body).Member;
         if (member is PropertyInfo prop)
         {
            return new PropertyMustBe<T, TProp>(@this.Value, prop.Name, (TProp)prop.GetValue(@this.Value));
         }

         throw new MustBeException("Must.SwitchTo<TOld, T, TProp>() nextSelector must be a property selector.");
      }

      #endregion
   }
}