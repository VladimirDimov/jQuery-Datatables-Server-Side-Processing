namespace JQDT.DataProcessing
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.Extensions;
    using JQDT.Models;

    /// <summary>
    /// Sort data processor.
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase" />
    internal class SortDataProcessor : DataProcessBase
    {
        private const string ASC = "asc";

        private const string INVALID_PROPERTY_NAME_EXCEPTION = "Invalid property name. The property {0} does not exist in the model.";

        private const string MissingColumnNameException = @"Missing column name for column with index {0}. Make sure that the data property of the column is configured appropriately as described in jQuery Datatables documentation.";
        private const string InvalidPropertyTypeException = "Invalid property type: {0}. Can sort only by simple types.";

        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns>
        ///   <see cref="IQueryable{object}" />
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when invalid property name is passed</exception>
        protected override IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            var modelType = requestInfoModel.Helpers.ModelType;

            var isFirst = true;
            foreach (var orderColumn in requestInfoModel.TableParameters.Order)
            {
                var colName = requestInfoModel.TableParameters.Columns[orderColumn.Column].Data;

                if (string.IsNullOrEmpty(colName))
                {
                    var ex = new ArgumentException(string.Format(MissingColumnNameException, orderColumn.Column));
                    ex.HelpLink = "https://datatables.net/examples/ajax/objects.html";

                    throw ex;
                }

                var isAsc = orderColumn.Dir == ASC;

                var propInfoPath = modelType.GetPropertyInfoPath(colName);
                var propInfo = propInfoPath.Last();
                if (propInfoPath == null)
                {
                    throw new ArgumentException(string.Format(INVALID_PROPERTY_NAME_EXCEPTION, colName));
                }

                if (propInfo.PropertyType.IsCLRLibraryType())
                {
                    throw new ArgumentException(string.Format(InvalidPropertyTypeException, propInfo.PropertyType.FullName));
                }

                var propType = propInfo.PropertyType;

                var lambdaExpr = this.OrderByExpression(propType, isAsc, isFirst);
                var propertySelectExpr = modelType.GetPropertySelectExpression(propInfoPath);

                if (isFirst)
                {
                    data = (IQueryable<object>)lambdaExpr.Compile().DynamicInvoke(data, propertySelectExpr);
                }
                else
                {
                    data = (IOrderedQueryable<object>)lambdaExpr.Compile().DynamicInvoke(data, propertySelectExpr);
                }

                isFirst = false;
            }

            return data;
        }

        private LambdaExpression OrderByExpression(Type propType, bool isAscending, bool isFirst)
        {
            // data
            var dataType = (isFirst ? typeof(IQueryable<>) : typeof(IOrderedQueryable<>)).MakeGenericType(typeof(object));
            var dataExpr = Expression.Parameter(dataType, "x");

            // selector
            var funcGenericType = typeof(Func<,>).MakeGenericType(typeof(object), propType);
            var selectorParamExpr = Expression.Parameter(typeof(Expression<>).MakeGenericType(funcGenericType), "selector");

            // data.OrderBy(selector)
            var orderMethodName = this.GetOrderMethodName(isFirst, isAscending);
            var orderByExpr = Expression.Call(
                typeof(Queryable),
                orderMethodName,
                new Type[] { typeof(object), propType },
                dataExpr,
                selectorParamExpr);

            // data, selector => data.OrderBy(selector)
            var lambda = Expression.Lambda(orderByExpr, dataExpr, selectorParamExpr);

            return lambda;
        }

        /// <summary>
        /// Generates the order method name.
        /// Possible results are OrderBy, OrderByDescending, ThenBy and ThenByDescending.
        /// </summary>
        /// <param name="isFirst">Set to <c>true</c> when order by first property.</param>
        /// <param name="isAscending">if set to <c>true</c> [is ascending].</param>
        /// <returns>method name</returns>
        private string GetOrderMethodName(bool isFirst, bool isAscending)
        {
            var methodPrefix = isFirst ? "OrderBy" : "ThenBy";
            var methodSuffix = isAscending ? string.Empty : "Descending";
            var orderMethodName = $"{methodPrefix}{methodSuffix}";

            return orderMethodName;
        }
    }
}