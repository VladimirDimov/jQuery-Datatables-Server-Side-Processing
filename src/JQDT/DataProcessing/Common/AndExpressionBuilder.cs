namespace JQDT.DataProcessing.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Builds And clause binary expression
    /// </summary>
    internal class AndExpressionBuilder
    {
        /// <summary>
        /// Joins the expressions inside a collection of <see cref="Expression"/> with AND clause.
        /// </summary>
        /// <param name="expressions">The contain expression collection.</param>
        /// <returns>Joined <see cref="Expression"/></returns>
        internal Expression BuildExpression(List<Expression> expressions)
        {
            if (expressions.Count == 0)
            {
                return null;
            }

            if (expressions.Count == 1)
            {
                return expressions.First();
            }

            var numberOfExpressions = expressions.Count;
            var counter = 0;
            Expression andExpr = null;
            do
            {
                andExpr = Expression.AndAlso(andExpr ?? expressions[counter], expressions[counter + 1]);

                counter++;
            }
            while (counter < numberOfExpressions - 1);

            return andExpr;
        }
    }
}