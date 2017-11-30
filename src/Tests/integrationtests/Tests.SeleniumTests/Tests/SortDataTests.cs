using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using OpenQA.Selenium;
using TestData.Models;
using Tests.Helpers;
using Tests.SeleniumTests.Common;
using Tests.SeleniumTests.Enumerations;

namespace Tests.SeleniumTests.Tests
{
    public class SortDataTests
    {
        private IWebDriver driver;
        private Navigator navigator;

        [SetUp]
        public void SetUp()
        {
            this.driver = DriverSingletonProvider.GetDriver();
            this.navigator = new Navigator(driver);
        }

        [Test]
        [TestCase("StringProperty", SortDirectionsEnum.Asc)]
        [TestCase("StringProperty", SortDirectionsEnum.Desc)]
        [TestCase("CharProperty", SortDirectionsEnum.Asc)]
        [TestCase("CharProperty", SortDirectionsEnum.Desc)]
        [TestCase("CharNullable", SortDirectionsEnum.Asc)]
        [TestCase("CharNullable", SortDirectionsEnum.Desc)]
        [TestCase("Nested Model StringProperty", SortDirectionsEnum.Asc)]
        [TestCase("Nested Model StringProperty", SortDirectionsEnum.Desc)]
        [TestCase("Nested Model CharProperty", SortDirectionsEnum.Asc)]
        [TestCase("Nested Model CharProperty", SortDirectionsEnum.Desc)]
        [TestCase("Nested Model CharNullable", SortDirectionsEnum.Asc)]
        [TestCase("Nested Model CharNullable", SortDirectionsEnum.Desc)]
        public void Sort_SholdWorkAppropriateForTextTypes(string columnName, SortDirectionsEnum direction)
        {
            this.navigator.AllTypesDataPage().GoTo();
            var tableElement = new TableElement("table", this.driver);
            tableElement.ClickSortButton(columnName, direction);

            // Assert that rows are in correct order for the first page
            AssertTextPropertyOrder(columnName, direction, tableElement);
            tableElement.GoToLastPage();
            // Assert that rows are in correct order for the last page
            AssertTextPropertyOrder(columnName, direction, tableElement);
        }

        [Test]
        [TestCase(nameof(AllTypesModel.Integer), SortDirectionsEnum.Asc, typeof(int))]
        [TestCase(nameof(AllTypesModel.IntegerNullable), SortDirectionsEnum.Asc, typeof(int?))]
        [TestCase(nameof(AllTypesModel.UInt), SortDirectionsEnum.Asc, typeof(uint))]
        [TestCase(nameof(AllTypesModel.UIntNullable), SortDirectionsEnum.Asc, typeof(uint?))]
        [TestCase(nameof(AllTypesModel.Long), SortDirectionsEnum.Asc, typeof(long))]
        [TestCase(nameof(AllTypesModel.LongNullable), SortDirectionsEnum.Asc, typeof(long?))]
        [TestCase(nameof(AllTypesModel.ULong), SortDirectionsEnum.Asc, typeof(ulong))]
        [TestCase(nameof(AllTypesModel.ULongNullable), SortDirectionsEnum.Asc, typeof(ulong?))]
        [TestCase(nameof(AllTypesModel.Short), SortDirectionsEnum.Asc, typeof(short))]
        [TestCase(nameof(AllTypesModel.ShortNullable), SortDirectionsEnum.Asc, typeof(short?))]
        [TestCase(nameof(AllTypesModel.UShort), SortDirectionsEnum.Asc, typeof(ushort))]
        [TestCase(nameof(AllTypesModel.UShortNullable), SortDirectionsEnum.Asc, typeof(ushort?))]
        [TestCase(nameof(AllTypesModel.ByteProperty), SortDirectionsEnum.Asc, typeof(byte))]
        [TestCase(nameof(AllTypesModel.ByteNullable), SortDirectionsEnum.Asc, typeof(byte?))]
        [TestCase(nameof(AllTypesModel.SByteProperty), SortDirectionsEnum.Asc, typeof(sbyte))]
        [TestCase(nameof(AllTypesModel.SByteNullable), SortDirectionsEnum.Asc, typeof(sbyte?))]
        [TestCase(nameof(AllTypesModel.DoubleProperty), SortDirectionsEnum.Asc, typeof(double))]
        [TestCase(nameof(AllTypesModel.DoubleNullable), SortDirectionsEnum.Asc, typeof(double?))]
        [TestCase(nameof(AllTypesModel.DecimalProperty), SortDirectionsEnum.Asc, typeof(decimal))]
        [TestCase(nameof(AllTypesModel.DecimalNullable), SortDirectionsEnum.Asc, typeof(decimal?))]
        [TestCase(nameof(AllTypesModel.DateTimeProperty), SortDirectionsEnum.Asc, typeof(DateTime))]
        [TestCase(nameof(AllTypesModel.DateTimeNullable), SortDirectionsEnum.Asc, typeof(DateTime?))]
        [TestCase(nameof(AllTypesModel.DateTimeOffsetProperty), SortDirectionsEnum.Asc, typeof(DateTimeOffset))]
        [TestCase(nameof(AllTypesModel.DateTimeOffsetNullable), SortDirectionsEnum.Asc, typeof(DateTimeOffset?))]
        [TestCase(nameof(AllTypesModel.BooleanProperty), SortDirectionsEnum.Asc, typeof(bool))]
        [TestCase(nameof(AllTypesModel.BooleanNullable), SortDirectionsEnum.Asc, typeof(bool?))]
        [TestCase(nameof(AllTypesModel.CharProperty), SortDirectionsEnum.Asc, typeof(char))]
        [TestCase(nameof(AllTypesModel.CharNullable), SortDirectionsEnum.Asc, typeof(char?))]
        // ---------------------------------------------------------------------------------------------------
        [TestCase(nameof(AllTypesModel.Integer), SortDirectionsEnum.Desc, typeof(int))]
        [TestCase(nameof(AllTypesModel.IntegerNullable), SortDirectionsEnum.Desc, typeof(int?))]
        [TestCase(nameof(AllTypesModel.UInt), SortDirectionsEnum.Desc, typeof(uint))]
        [TestCase(nameof(AllTypesModel.UIntNullable), SortDirectionsEnum.Desc, typeof(uint?))]
        [TestCase(nameof(AllTypesModel.Long), SortDirectionsEnum.Desc, typeof(long))]
        [TestCase(nameof(AllTypesModel.LongNullable), SortDirectionsEnum.Desc, typeof(long?))]
        [TestCase(nameof(AllTypesModel.ULong), SortDirectionsEnum.Desc, typeof(ulong))]
        [TestCase(nameof(AllTypesModel.ULongNullable), SortDirectionsEnum.Desc, typeof(ulong?))]
        [TestCase(nameof(AllTypesModel.Short), SortDirectionsEnum.Desc, typeof(short))]
        [TestCase(nameof(AllTypesModel.ShortNullable), SortDirectionsEnum.Desc, typeof(short?))]
        [TestCase(nameof(AllTypesModel.UShort), SortDirectionsEnum.Desc, typeof(ushort))]
        [TestCase(nameof(AllTypesModel.UShortNullable), SortDirectionsEnum.Desc, typeof(ushort?))]
        [TestCase(nameof(AllTypesModel.ByteProperty), SortDirectionsEnum.Desc, typeof(byte))]
        [TestCase(nameof(AllTypesModel.ByteNullable), SortDirectionsEnum.Desc, typeof(byte?))]
        [TestCase(nameof(AllTypesModel.SByteProperty), SortDirectionsEnum.Desc, typeof(sbyte))]
        [TestCase(nameof(AllTypesModel.SByteNullable), SortDirectionsEnum.Desc, typeof(sbyte?))]
        [TestCase(nameof(AllTypesModel.DoubleProperty), SortDirectionsEnum.Desc, typeof(double))]
        [TestCase(nameof(AllTypesModel.DoubleNullable), SortDirectionsEnum.Desc, typeof(double?))]
        [TestCase(nameof(AllTypesModel.DecimalProperty), SortDirectionsEnum.Desc, typeof(decimal))]
        [TestCase(nameof(AllTypesModel.DecimalNullable), SortDirectionsEnum.Desc, typeof(decimal?))]
        [TestCase(nameof(AllTypesModel.DateTimeProperty), SortDirectionsEnum.Desc, typeof(DateTime))]
        [TestCase(nameof(AllTypesModel.DateTimeNullable), SortDirectionsEnum.Desc, typeof(DateTime?))]
        [TestCase(nameof(AllTypesModel.DateTimeOffsetProperty), SortDirectionsEnum.Desc, typeof(DateTimeOffset))]
        [TestCase(nameof(AllTypesModel.DateTimeOffsetNullable), SortDirectionsEnum.Desc, typeof(DateTimeOffset?))]
        [TestCase(nameof(AllTypesModel.BooleanProperty), SortDirectionsEnum.Desc, typeof(bool))]
        [TestCase(nameof(AllTypesModel.BooleanNullable), SortDirectionsEnum.Desc, typeof(bool?))]
        [TestCase(nameof(AllTypesModel.CharProperty), SortDirectionsEnum.Desc, typeof(char))]
        [TestCase(nameof(AllTypesModel.CharNullable), SortDirectionsEnum.Desc, typeof(char?))]
        // ---------------------------------------------------------------------------------------------------
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.Integer), SortDirectionsEnum.Asc, typeof(int))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.IntegerNullable), SortDirectionsEnum.Asc, typeof(int?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.UInt), SortDirectionsEnum.Asc, typeof(uint))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.UIntNullable), SortDirectionsEnum.Asc, typeof(uint?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.Long), SortDirectionsEnum.Asc, typeof(long))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.LongNullable), SortDirectionsEnum.Asc, typeof(long?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.ULong), SortDirectionsEnum.Asc, typeof(ulong))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.ULongNullable), SortDirectionsEnum.Asc, typeof(ulong?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.Short), SortDirectionsEnum.Asc, typeof(short))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.ShortNullable), SortDirectionsEnum.Asc, typeof(short?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.UShort), SortDirectionsEnum.Asc, typeof(ushort))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.UShortNullable), SortDirectionsEnum.Asc, typeof(ushort?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.ByteProperty), SortDirectionsEnum.Asc, typeof(byte))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.ByteNullable), SortDirectionsEnum.Asc, typeof(byte?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.SByteProperty), SortDirectionsEnum.Asc, typeof(sbyte))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.SByteNullable), SortDirectionsEnum.Asc, typeof(sbyte?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DoubleProperty), SortDirectionsEnum.Asc, typeof(double))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DoubleNullable), SortDirectionsEnum.Asc, typeof(double?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DecimalProperty), SortDirectionsEnum.Asc, typeof(decimal))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DecimalNullable), SortDirectionsEnum.Asc, typeof(decimal?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DateTimeProperty), SortDirectionsEnum.Asc, typeof(DateTime))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DateTimeNullable), SortDirectionsEnum.Asc, typeof(DateTime?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DateTimeOffsetProperty), SortDirectionsEnum.Asc, typeof(DateTimeOffset))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DateTimeOffsetNullable), SortDirectionsEnum.Asc, typeof(DateTimeOffset?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.BooleanProperty), SortDirectionsEnum.Asc, typeof(bool))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.BooleanNullable), SortDirectionsEnum.Asc, typeof(bool?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.CharProperty), SortDirectionsEnum.Asc, typeof(char))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.CharNullable), SortDirectionsEnum.Asc, typeof(char?))]
        // ---------------------------------------------------------------------------------------------------
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.Integer), SortDirectionsEnum.Desc, typeof(int))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.IntegerNullable), SortDirectionsEnum.Desc, typeof(int?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.UInt), SortDirectionsEnum.Desc, typeof(uint))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.UIntNullable), SortDirectionsEnum.Desc, typeof(uint?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.Long), SortDirectionsEnum.Desc, typeof(long))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.LongNullable), SortDirectionsEnum.Desc, typeof(long?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.ULong), SortDirectionsEnum.Desc, typeof(ulong))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.ULongNullable), SortDirectionsEnum.Desc, typeof(ulong?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.Short), SortDirectionsEnum.Desc, typeof(short))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.ShortNullable), SortDirectionsEnum.Desc, typeof(short?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.UShort), SortDirectionsEnum.Desc, typeof(ushort))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.UShortNullable), SortDirectionsEnum.Desc, typeof(ushort?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.ByteProperty), SortDirectionsEnum.Desc, typeof(byte))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.ByteNullable), SortDirectionsEnum.Desc, typeof(byte?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.SByteProperty), SortDirectionsEnum.Desc, typeof(sbyte))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.SByteNullable), SortDirectionsEnum.Desc, typeof(sbyte?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DoubleProperty), SortDirectionsEnum.Desc, typeof(double))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DoubleNullable), SortDirectionsEnum.Desc, typeof(double?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DecimalProperty), SortDirectionsEnum.Desc, typeof(decimal))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DecimalNullable), SortDirectionsEnum.Desc, typeof(decimal?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DateTimeProperty), SortDirectionsEnum.Desc, typeof(DateTime))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DateTimeNullable), SortDirectionsEnum.Desc, typeof(DateTime?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DateTimeOffsetProperty), SortDirectionsEnum.Desc, typeof(DateTimeOffset))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.DateTimeOffsetNullable), SortDirectionsEnum.Desc, typeof(DateTimeOffset?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.BooleanProperty), SortDirectionsEnum.Desc, typeof(bool))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.BooleanNullable), SortDirectionsEnum.Desc, typeof(bool?))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.CharProperty), SortDirectionsEnum.Desc, typeof(char))]
        [TestCase(nameof(AllTypesModel.NestedModel) + "." + nameof(AllTypesModel.CharNullable), SortDirectionsEnum.Desc, typeof(char?))]
        public void Sort_SholdWorkAppropriateForNonTextTypes(string columnName, SortDirectionsEnum direction, Type propertyType)
        {
            this.navigator.AllTypesDataPage().GoTo();
            var tableElement = new TableElement("table", this.driver);
            string columnHeader = columnName.StartsWith("NestedModel") ? this.GetHeaderForNestedModel(columnName) : columnName;
            tableElement.ClickSortButton(columnHeader, direction);

            AssertNonTextPropertyOrder(columnName, columnHeader, direction, propertyType, tableElement);
            tableElement.GoToLastPage();
            AssertNonTextPropertyOrder(columnName, columnHeader, direction, propertyType, tableElement);
        }

        private string GetHeaderForNestedModel(string columnName)
        {
            var propName = columnName.Split('.').Last();

            return $"Nested Model {propName}";
        }

        private void AssertNonTextPropertyOrder(string columnName, string columnHeader, SortDirectionsEnum direction, Type propertyType, TableElement tableElement)
        {
            var actualColumnValues = tableElement.GetColumnRowValues(columnHeader);

            var stringParseFunction = this.GetStringParseFunction(columnName, propertyType);
            var expectedColumnValues = direction == SortDirectionsEnum.Asc ?
                actualColumnValues.OrderBy(stringParseFunction) :
                actualColumnValues.OrderByDescending(stringParseFunction);

            Assert.IsNotEmpty(actualColumnValues);
            Assert.IsTrue(expectedColumnValues.SequenceEqual(actualColumnValues));
        }

        private Func<string, object> GetStringParseFunction(string property, Type toType)
        {
            if (toType == typeof(uint?))
            {
                return this.NullableTryParseFunction<uint>();
            }
            else if (toType == typeof(int?))
            {
                return this.NullableTryParseFunction<int>();
            }
            else if (toType == typeof(long?))
            {
                return this.NullableTryParseFunction<long>();
            }
            else if (toType == typeof(ulong?))
            {
                return this.NullableTryParseFunction<ulong>();
            }
            else if (toType == typeof(short?))
            {
                return this.NullableTryParseFunction<short>();
            }
            else if (toType == typeof(ushort?))
            {
                return this.NullableTryParseFunction<ushort>();
            }
            else if (toType == typeof(byte?))
            {
                return this.NullableTryParseFunction<byte>();
            }
            else if (toType == typeof(sbyte?))
            {
                return this.NullableTryParseFunction<sbyte>();
            }
            else if (toType == typeof(double?))
            {
                return this.NullableTryParseFunction<double>();
            }
            else if (toType == typeof(decimal?))
            {
                return this.NullableTryParseFunction<decimal>();
            }
            else if (toType == typeof(DateTime?))
            {
                return this.NullableTryParseFunction<DateTime>();
            }
            else if (toType == typeof(DateTimeOffset?))
            {
                return this.NullableTryParseFunction<DateTimeOffset>();
            }
            else if (toType == typeof(bool?))
            {
                return this.NullableTryParseFunction<bool>();
            }
            else if (toType == typeof(char?))
            {
                return this.NullableTryParseFunction<char>();
            }

            var stringParam = Expression.Parameter(typeof(string), "x");
            var parseMethodInfo = toType.GetMethods().Where(x => x.Name == "Parse").First(x => x.GetParameters().Count() == 1);
            var parseMethodCallExpr = Expression.Call(null, parseMethodInfo, stringParam);
            var convertExpr = Expression.Convert(parseMethodCallExpr, typeof(object));
            var lambda = Expression.Lambda(convertExpr, stringParam);

            return (Func<string, object>)lambda.Compile();
        }

        private Func<string, object> NullableTryParseFunction<T>()
            where T : struct
        {
            return new Func<string, object>(x =>
            {
                Nullable<T> i = null;
                var mi = typeof(T).GetMethods().Where(m => m.Name == "TryParse").Where(m => m.GetParameters().Count() == 2).First();
                var success = (bool)mi.Invoke(null, new object[] { x, i });

                return success ? (Nullable<T>)i : null;
            });
        }

        private static void AssertTextPropertyOrder(string columnName, SortDirectionsEnum direction, TableElement tableElement)
        {
            var actualColumnValues = tableElement.GetColumnRowValues(columnName);

            var expectedColumnValues = direction == SortDirectionsEnum.Asc ?
                actualColumnValues.OrderBy(x => x) :
                actualColumnValues.OrderByDescending(x => x);

            Assert.IsNotEmpty(actualColumnValues);
            Assert.IsTrue(expectedColumnValues.SequenceEqual(actualColumnValues));
        }
    }
}