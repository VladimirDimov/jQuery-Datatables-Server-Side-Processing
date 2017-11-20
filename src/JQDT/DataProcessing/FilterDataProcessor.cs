namespace JQDT.DataProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.Extensions;
    using JQDT.Models;

    /// <summary>
    /// Filters the data by a substring value. Looks for the substring in all public properties of the data model.
    /// </summary>
    internal class FilterDataProcessor<T> : DataProcessBase<T>
    {
        private const string NoSearchablePropertiesException = "A search value has been provided but no searchable properties were found. Make sure that the data property of the column is configured appropriately as described in jQuery Datatables documentation.";
        private const string HelpLink = "https://datatables.net/examples/ajax/objects.html";

        private RequestInfoModel requestInfoModel;

        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns>
        ///   <see cref="IQueryable{T}" />
        /// </returns>
        protected override IQueryable<T> OnProcessData(IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            if (string.IsNullOrWhiteSpace(requestInfoModel.TableParameters.Search.Value))
            {
                return data;
            }

            this.requestInfoModel = requestInfoModel;

            var expr = this.BuildExpression(requestInfoModel.Helpers.ModelType, requestInfoModel.TableParameters.Search.Value);
            data = data.Where(expr);

            return data;
        }

        private Expression<Func<T, bool>> BuildExpression(Type modelType, string search)
        {
            // x
            var modelParamExpr = Expression.Parameter(typeof(T), "model");
            var properties = ((System.Reflection.TypeInfo)modelType).DeclaredProperties;// TODO: Remove this line
            var containExpressionCollection = new List<MethodCallExpression>();

            var searchableProperties = this.requestInfoModel.TableParameters.Columns
                .Where(col => col.Searchable)
                .Select(col => col.Data);

            if (!searchableProperties.Any())
            {
                throw new ArgumentException(NoSearchablePropertiesException)
                {
                    HelpLink = HelpLink
                };
            }

            foreach (var propertyPath in searchableProperties)
            {
                var currentPropertyContainsExpression = this.GetSinglePropertyContainsExpression(modelType, search, propertyPath, modelParamExpr);
                containExpressionCollection.Add(currentPropertyContainsExpression);
            }

            // If the search is performed on more than one property the Contain expressions must be joined with OR operator
            Expression joinedExpressions = null;
            if (containExpressionCollection.Count > 1)
            {
                joinedExpressions = this.GetOrExpr(containExpressionCollection);
            }
            else
            {
                joinedExpressions = containExpressionCollection.Single();
            }

            var lambda = Expression.Lambda(joinedExpressions, modelParamExpr);

            return (Expression<Func<T, bool>>)lambda;
        }

        private Expression GetOrExpr(List<MethodCallExpression> containExpressionCollection)
        {
            var numberOfExpressions = containExpressionCollection.Count;
            var counter = 0;
            Expression orExpr = null;
            do
            {
                orExpr = Expression.Or(orExpr ?? containExpressionCollection[counter], containExpressionCollection[counter + 1]);

                counter++;
            }
            while (counter < numberOfExpressions - 1);

            return orExpr;
        }

        // Returns the "Contains" expression for a single property
        private MethodCallExpression GetSinglePropertyContainsExpression(Type modelType, string search, string propertyPath, ParameterExpression modelParamExpr)
        {
            // searchVal
            var searchValExpr = Expression.Constant(search.ToLower());
            // (TypeOfX)x
            // var convertExpr = Expression.Convert(modelParamExpr, modelType);

            // x.Name
            var propExpr = modelParamExpr.NestedProperty(propertyPath);

            // x.Name.ToString()
            //var toStringMethodInfo = typeof(T).GetMethod("ToString");
            //var toStringExpr = Expression.Call(propExpr, toStringMethodInfo);
            var toStringExpr = this.GetStringRepressentationExpression(propExpr);

            // x.Name.ToString().ToLower()
            var toLowerMethodInfo = typeof(string).GetMethods().Where(m => m.Name == "ToLower" && !m.GetParameters().Any()).First();
            var toLowerExpr = Expression.Call(toStringExpr, toLowerMethodInfo);

            // x.Name.ToString().Contains()
            var containsMethodInfo = typeof(string).GetMethod("Contains");
            var containsExpr = Expression.Call(toLowerExpr, containsMethodInfo, searchValExpr);

            return containsExpr;
        }

        private Expression GetStringRepressentationExpression(MemberExpression memberExpr)
        {
            if (memberExpr.Type == typeof(string))
            {
                return memberExpr;
            }
            else if (memberExpr.Type == typeof(int) || memberExpr.Type == typeof(long) || memberExpr.Type == typeof(double) || memberExpr.Type == typeof(int?))
            {
                // SqlFunctions.StringConvert((decimal)x.Property)
                var stringConvertMethodInfo = typeof(SqlFunctions).GetMethods()
                    .Where(m =>
                    {
                        if (m.Name != "StringConvert") return false;
                        var parameters = m.GetParameters();
                        var numberOfParameters = parameters.Count();
                        if (numberOfParameters != 1)
                        {
                            return false;
                        }

                        if (parameters.First().ParameterType != typeof(decimal?))
                        {
                            return false;
                        }

                        return true;
                    }).Single();

                var castToDecimalExpr = Expression.Convert(memberExpr, typeof(decimal?));
                var stringConvertExpr = Expression.Call(stringConvertMethodInfo, castToDecimalExpr);

                return stringConvertExpr;
            }

            throw new NotImplementedException($"Cannot filter by type: {memberExpr.Type.FullName}");
        }
    }
}