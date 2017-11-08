namespace JQDT.DataProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
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

                var propInfoPath = this.GetPropertyInfoPath(modelType, colName);
                var propInfo = propInfoPath.Last();
                if (propInfoPath == null)
                {
                    throw new ArgumentException($"Invalid property name. The property {colName} does not exist in the model.");
                }

                var propType = propInfo.PropertyType;

                var lambdaExpr = this.OrderByExpression(propType, isAsc, isFirst);
                var propertySelectExpr = this.GetPropertySelectExpression(modelType, propInfoPath);

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

        /// <summary>
        /// Returns collection of <see cref="PropertyInfo"/>. The collection contains the <see cref="PropertyInfo"/> of
        /// the model properties from the parent properties to the target property.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">
        /// Name of the property. May contain nested properties delimited by ".".
        /// Example: Address.Street.Number
        /// </param>
        /// <returns><see cref="ICollection{PropertyInfo}"/></returns>
        /// <exception cref="ArgumentException">Thrown on invalid property name.</exception>
        private ICollection<PropertyInfo> GetPropertyInfoPath(Type model, string propertyName)
        {
            var propertyNamePath = propertyName.Split('.');
            Type currentModelType = model;
            var propertyInfoPath = new List<PropertyInfo>();
            foreach (var propName in propertyNamePath)
            {
                var propInfo = currentModelType.GetProperties().FirstOrDefault(p => p.Name == propName);
                if (propInfo == null)
                {
                    throw new ArgumentException($"Invalid property name. The property {propertyName} does not exist in the model. Throw on {propName}");
                }

                currentModelType = propInfo.PropertyType;
                propertyInfoPath.Add(propInfo);
            }

            return propertyInfoPath;
        }

        /// <summary>
        /// Gets the property select expression.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyInfoPath">Collection of <see cref="PropertyInfo"/> describing the path from parent to target property.
        /// </param>
        /// <returns><see cref="LambdaExpression"/> Ex: "x => (ModelType)x.Property1.Property2"</returns>
        private LambdaExpression GetPropertySelectExpression(Type modelType, IEnumerable<PropertyInfo> propertyInfoPath)
        {
            // x
            ParameterExpression xExpr = Expression.Parameter(typeof(object), "x");

            // (ModelType)x
            var castExpr = Expression.Convert(xExpr, modelType);

            // (ModelType)x.Property
            MemberExpression propExpression = null;
            foreach (var propInfo in propertyInfoPath)
            {
                propExpression = propExpression == null ?
                    Expression.Property(castExpr, propInfo) :
                    Expression.Property(propExpression, propInfo);
            }

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