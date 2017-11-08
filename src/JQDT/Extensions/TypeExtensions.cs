﻿namespace JQDT.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TypeExptensions
    {
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
        public static ICollection<PropertyInfo> GetPropertyInfoPath(this Type model, string propertyName)
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
    }
}