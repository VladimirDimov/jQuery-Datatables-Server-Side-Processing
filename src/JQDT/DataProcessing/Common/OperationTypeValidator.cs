using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JQDT.Enumerations;
using JQDT.Exceptions;
using JQDT.Extensions;
using JQDT.Models;

namespace JQDT.DataProcessing.Common
{
    class OperationTypeValidator
    {
        /// <summary>
        /// Validates the type of the property for operation type.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <exception cref="JQDT.Exceptions.InvalidTypeForOperationException">Throw when property type is invalid for the requested operation.</exception>
        internal void ValidatePropertyType(Type propertyType, FilterTypes filterType)
        {
            bool isValidForOperation = true;
            var operationType = this.GetOperationType(filterType);

            isValidForOperation = propertyType.IsValidForOperation(operationType);

            if (!isValidForOperation)
            {
                throw new InvalidTypeForOperationException(propertyType, operationType);
            }
        }

        /// <summary>
        /// Gets the type of the operation based on provided <see cref="FilterTypes"/>.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <returns>Corresponding <see cref="OperationTypesEnum"/></returns>
        private OperationTypesEnum GetOperationType(FilterTypes filterType)
        {
            switch (filterType)
            {
                case FilterTypes.gte:
                case FilterTypes.gt:
                case FilterTypes.lt:
                case FilterTypes.lte:
                    return OperationTypesEnum.Range;

                case FilterTypes.eq:
                    return OperationTypesEnum.Equals;

                default:
                    return OperationTypesEnum.Default;
            }
        }
    }
}
