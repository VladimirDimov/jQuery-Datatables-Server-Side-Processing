namespace JQDT.DataProcessing
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.Models;

    /// <summary>
    /// Sort data processor.
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase" />
    internal class SortDataProcessor : DataProcessBase
    {
        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns>
        ///   <see cref="IQueryable{object}" />
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when invalid property name is passed</exception>
        public override IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            var genericType = data.GetType();
            var modelType = requestInfoModel.Helpers.ModelType;
            IQueryable<object> orderedData = data.Select(x => x);
            var isFirst = true;
            foreach (var orderColumn in requestInfoModel.TableParameters.Order)
            {
                var colName = requestInfoModel.TableParameters.Columns[orderColumn.Column].Data;
                var isAsc = orderColumn.Dir == "asc";

                // TODO: Extract to GetPropertyType method
                var propInfo = modelType.GetProperties().FirstOrDefault(p => p.Name == colName);
                if (propInfo == null)
                {
                    throw new ArgumentException($"Invalid property name. The property {colName} does not exist in the model.");
                }

                var propType = propInfo.PropertyType;

                var lambdaExpr = this.OrderByExpression(propType, isAsc, isFirst);
                var propertySelectExpr = this.GetPropertySelectExpression(modelType, colName);

                if (isFirst)
                {
                    orderedData = (IQueryable<object>)lambdaExpr.Compile().DynamicInvoke(data, propertySelectExpr);
                }
                else
                {
                    orderedData = (IOrderedQueryable<object>)lambdaExpr.Compile().DynamicInvoke(orderedData, propertySelectExpr);
                }

                isFirst = false;
            }

            return orderedData;
        }

        private LambdaExpression GetPropertySelectExpression(Type modelType, string propertyName)
        {
            // x
            ParameterExpression xExpr = Expression.Parameter(typeof(object), "x");

            // (ModelType)x
            var castExpr = Expression.Convert(xExpr, modelType);

            // (ModelType)x.Property
            var propExpression = Expression.Property(castExpr, propertyName);

            // x => (ModelType)x.Property
            var lambda = Expression.Lambda(propExpression, xExpr);

            return lambda;
        }

        private LambdaExpression OrderByExpression(Type propType, bool isAsending, bool isFirst)
        {
            // data
            var dataType = (isFirst ? typeof(IQueryable<>) : typeof(IOrderedQueryable<>)).MakeGenericType(typeof(object));
            var dataExpr = Expression.Parameter(dataType, "x");

            // selector
            var funcGenericType = typeof(Func<,>).MakeGenericType(typeof(object), propType);
            var selectorParamExpr = Expression.Parameter(typeof(Expression<>).MakeGenericType(funcGenericType), "selector");

            // data.OrderBy(selector)
            var methodPrefix = isFirst ? "OrderBy" : "ThenBy";
            var methodSuffix = isAsending ? string.Empty : "Descending";
            var orderMethodName = $"{methodPrefix}{methodSuffix}";

            // TODO: Extract ThenBy on separate method. ThenBy is applied on IOrderedQueryable
            var orderByExpr = Expression.Call(typeof(Queryable), orderMethodName, new Type[] { typeof(object), propType }, dataExpr, selectorParamExpr);

            // data, selector => data.OrderBy(selector)
            var lambda = Expression.Lambda(orderByExpr, dataExpr, selectorParamExpr);

            return lambda;
        }
    }
}