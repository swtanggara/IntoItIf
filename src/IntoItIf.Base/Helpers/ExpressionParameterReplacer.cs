namespace IntoItIf.Base.Helpers
{
   using System.Collections.Generic;
   using System.Linq.Expressions;

   public class ExpressionParameterReplacer : ExpressionVisitor
   {
      #region Constructors and Destructors

      public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
      {
         ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
         for (var i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
            ParameterReplacements.Add(fromParameters[i], toParameters[i]);
      }

      #endregion

      #region Properties

      private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; }

      #endregion

      #region Methods

      protected override Expression VisitParameter(ParameterExpression node)
      {
         if (ParameterReplacements.TryGetValue(node, out var replacement))
            node = replacement;
         return base.VisitParameter(node);
      }

      #endregion
   }
}