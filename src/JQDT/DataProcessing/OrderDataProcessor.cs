namespace JQDT.DataProcessing
{
    using JQDT.Models;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    internal class OrderDataProcessor : IDataProcess
    {
        public IQueryable<object> ProcessedData { get; set; }

        public IQueryable<object> ProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            var genericType = data.GetType();
            var modelType = requestInfoModel.Helpers.ModelType;
            IQueryable<object> orderedData = data.Select(x => x);
            var isFirst = true;
            foreach (var orderColumn in requestInfoModel.TableParameters.order)
            {
                var colName = requestInfoModel.TableParameters.columns[orderColumn.column].data;
                var isAsc = orderColumn.dir == "asc";
                var propType = modelType.GetProperty(colName).PropertyType;

                var lambdaExpr = OrderByExpression(propType, isAsc, isFirst);
                var propertySelectExpr = GetPropertySelectExpression(modelType, colName);

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
            // data, selector => data.OrderBy(selector)
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