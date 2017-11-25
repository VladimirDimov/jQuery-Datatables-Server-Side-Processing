﻿namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Dynamic;
    using JQDT.DataProcessing;
    using JQDT.DI;
    using JQDT.Models;
    using NUnit.Framework;
    using Tests.UnitTests.Common;
    using Tests.UnitTests.Models;

    internal class CustomFiltersDataProcessorTests
    {
        private IDataProcess<SimpleModel> filter;
        private IQueryable<SimpleModel> data;

        [SetUp]
        public void SetUp()
        {
            var resolver = new DependencyResolver();
            this.filter = resolver.GetCustomFiltersDataProcessor<SimpleModel>();
            this.data = DataGenerator.GenerateSimpleData(5000, 50);
        }

        [Test]
        // ---------------------------------------------------------------
        [TestCase("Integer", FilterTypes.gt, "25")]
        [TestCase("Integer", FilterTypes.gte, "25")]
        [TestCase("Integer", FilterTypes.lt, "25")]
        [TestCase("Integer", FilterTypes.lte, "25")]
        [TestCase("Integer", FilterTypes.eq, "25")]
        [TestCase("Integer", FilterTypes.eq, "-25")]
        [TestCase("Integer", FilterTypes.eq, int.MaxValue)]
        [TestCase("Integer", FilterTypes.eq, int.MinValue)]
        [TestCase("NestedModel.Integer", FilterTypes.gt, "25")]
        [TestCase("NestedModel.Integer", FilterTypes.gte, "25")]
        [TestCase("NestedModel.Integer", FilterTypes.lt, "25")]
        [TestCase("NestedModel.Integer", FilterTypes.lte, "25")]
        [TestCase("NestedModel.Integer", FilterTypes.eq, "25")]
        [TestCase("NestedModel.Integer", FilterTypes.eq, "-25")]
        [TestCase("NestedModel.Integer", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.Integer", FilterTypes.eq, int.MaxValue)]
        [TestCase("NestedModel.Integer", FilterTypes.eq, int.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("IntegerNullable", FilterTypes.gt, "25")]
        [TestCase("IntegerNullable", FilterTypes.gte, "25")]
        [TestCase("IntegerNullable", FilterTypes.lt, "25")]
        [TestCase("IntegerNullable", FilterTypes.lte, "25")]
        [TestCase("IntegerNullable", FilterTypes.eq, "25")]
        [TestCase("IntegerNullable", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.eq, int.MaxValue)]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.eq, int.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("UInt", FilterTypes.gt, "25")]
        [TestCase("UInt", FilterTypes.gte, "25")]
        [TestCase("UInt", FilterTypes.lt, "25")]
        [TestCase("UInt", FilterTypes.lte, "25")]
        [TestCase("UInt", FilterTypes.eq, "25")]
        [TestCase("UInt", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.UInt", FilterTypes.gt, "25")]
        [TestCase("NestedModel.UInt", FilterTypes.gte, "25")]
        [TestCase("NestedModel.UInt", FilterTypes.lt, "25")]
        [TestCase("NestedModel.UInt", FilterTypes.lte, "25")]
        [TestCase("NestedModel.UInt", FilterTypes.eq, "25")]
        [TestCase("NestedModel.UInt", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.UInt", FilterTypes.eq, uint.MaxValue)]
        [TestCase("NestedModel.UInt", FilterTypes.eq, uint.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("UIntNullable", FilterTypes.gt, "25")]
        [TestCase("UIntNullable", FilterTypes.gte, "25")]
        [TestCase("UIntNullable", FilterTypes.lt, "25")]
        [TestCase("UIntNullable", FilterTypes.lte, "25")]
        [TestCase("UIntNullable", FilterTypes.eq, "25")]
        [TestCase("UIntNullable", FilterTypes.eq, "99999999")]
        [TestCase("UIntNullable", FilterTypes.eq, uint.MaxValue)]
        [TestCase("UIntNullable", FilterTypes.eq, uint.MinValue)]
        [TestCase("NestedModel.UIntNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.eq, uint.MaxValue)]
        [TestCase("NestedModel.UIntNullable", FilterTypes.eq, uint.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("Long", FilterTypes.gt, "25")]
        [TestCase("Long", FilterTypes.gte, "25")]
        [TestCase("Long", FilterTypes.lt, "25")]
        [TestCase("Long", FilterTypes.lte, "25")]
        [TestCase("Long", FilterTypes.eq, "25")]
        [TestCase("Long", FilterTypes.eq, "-25")]
        [TestCase("Long", FilterTypes.eq, "99999999")]
        [TestCase("Long", FilterTypes.eq, "0")]
        [TestCase("Long", FilterTypes.eq, long.MaxValue)]
        [TestCase("Long", FilterTypes.eq, long.MinValue)]
        [TestCase("NestedModel.Long", FilterTypes.gt, "25")]
        [TestCase("NestedModel.Long", FilterTypes.gte, "25")]
        [TestCase("NestedModel.Long", FilterTypes.lt, "25")]
        [TestCase("NestedModel.Long", FilterTypes.lte, "25")]
        [TestCase("NestedModel.Long", FilterTypes.eq, "25")]
        [TestCase("NestedModel.Long", FilterTypes.eq, "-25")]
        [TestCase("NestedModel.Long", FilterTypes.eq, "0")]
        [TestCase("NestedModel.Long", FilterTypes.eq, long.MaxValue)]
        [TestCase("NestedModel.Long", FilterTypes.eq, long.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("LongNullable", FilterTypes.gt, "25")]
        [TestCase("LongNullable", FilterTypes.gte, "25")]
        [TestCase("LongNullable", FilterTypes.lt, "25")]
        [TestCase("LongNullable", FilterTypes.lte, "25")]
        [TestCase("LongNullable", FilterTypes.eq, "25")]
        [TestCase("LongNullable", FilterTypes.eq, "-25")]
        [TestCase("LongNullable", FilterTypes.eq, "99999999")]
        [TestCase("LongNullable", FilterTypes.eq, "0")]
        [TestCase("LongNullable", FilterTypes.eq, long.MaxValue)]
        [TestCase("LongNullable", FilterTypes.eq, long.MinValue)]
        [TestCase("NestedModel.LongNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.eq, "-25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.eq, "0")]
        [TestCase("NestedModel.LongNullable", FilterTypes.eq, long.MaxValue)]
        [TestCase("NestedModel.LongNullable", FilterTypes.eq, long.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("ULong", FilterTypes.gt, "25")]
        [TestCase("ULong", FilterTypes.gte, "25")]
        [TestCase("ULong", FilterTypes.lt, "25")]
        [TestCase("ULong", FilterTypes.lte, "25")]
        [TestCase("ULong", FilterTypes.eq, "25")]
        [TestCase("ULong", FilterTypes.eq, "99999999")]
        [TestCase("ULong", FilterTypes.eq, "0")]
        [TestCase("ULong", FilterTypes.eq, ulong.MaxValue)]
        [TestCase("ULong", FilterTypes.eq, ulong.MinValue)]
        [TestCase("NestedModel.ULong", FilterTypes.gt, "25")]
        [TestCase("NestedModel.ULong", FilterTypes.gte, "25")]
        [TestCase("NestedModel.ULong", FilterTypes.lt, "25")]
        [TestCase("NestedModel.ULong", FilterTypes.lte, "25")]
        [TestCase("NestedModel.ULong", FilterTypes.eq, "25")]
        [TestCase("NestedModel.ULong", FilterTypes.eq, "0")]
        [TestCase("NestedModel.ULong", FilterTypes.eq, ulong.MaxValue)]
        [TestCase("NestedModel.ULong", FilterTypes.eq, ulong.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("ULongNullable", FilterTypes.gt, "25")]
        [TestCase("ULongNullable", FilterTypes.gte, "25")]
        [TestCase("ULongNullable", FilterTypes.lt, "25")]
        [TestCase("ULongNullable", FilterTypes.lte, "25")]
        [TestCase("ULongNullable", FilterTypes.eq, "25")]
        [TestCase("ULongNullable", FilterTypes.eq, "99999999")]
        [TestCase("ULongNullable", FilterTypes.eq, "0")]
        [TestCase("ULongNullable", FilterTypes.eq, ulong.MaxValue)]
        [TestCase("ULongNullable", FilterTypes.eq, ulong.MinValue)]
        [TestCase("NestedModel.ULongNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.ULongNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.ULongNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.ULongNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.ULongNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.ULongNullable", FilterTypes.eq, "0")]
        [TestCase("NestedModel.ULongNullable", FilterTypes.eq, ulong.MaxValue)]
        [TestCase("NestedModel.ULongNullable", FilterTypes.eq, ulong.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("Short", FilterTypes.gt, "25")]
        [TestCase("Short", FilterTypes.gte, "25")]
        [TestCase("Short", FilterTypes.lt, "25")]
        [TestCase("Short", FilterTypes.lte, "25")]
        [TestCase("Short", FilterTypes.eq, "25")]
        [TestCase("Short", FilterTypes.eq, "0")]
        [TestCase("Short", FilterTypes.eq, short.MaxValue)]
        [TestCase("Short", FilterTypes.eq, short.MinValue)]
        [TestCase("NestedModel.Short", FilterTypes.gt, "25")]
        [TestCase("NestedModel.Short", FilterTypes.gte, "25")]
        [TestCase("NestedModel.Short", FilterTypes.lt, "25")]
        [TestCase("NestedModel.Short", FilterTypes.lte, "25")]
        [TestCase("NestedModel.Short", FilterTypes.eq, "25")]
        [TestCase("NestedModel.Short", FilterTypes.eq, "0")]
        [TestCase("NestedModel.Short", FilterTypes.eq, short.MaxValue)]
        [TestCase("NestedModel.Short", FilterTypes.eq, short.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("ShortNullable", FilterTypes.gt, "25")]
        [TestCase("ShortNullable", FilterTypes.gte, "25")]
        [TestCase("ShortNullable", FilterTypes.lt, "25")]
        [TestCase("ShortNullable", FilterTypes.lte, "25")]
        [TestCase("ShortNullable", FilterTypes.eq, "25")]
        [TestCase("ShortNullable", FilterTypes.eq, "-25")]
        [TestCase("ShortNullable", FilterTypes.eq, "0")]
        [TestCase("ShortNullable", FilterTypes.eq, short.MaxValue)]
        [TestCase("ShortNullable", FilterTypes.eq, short.MinValue)]
        [TestCase("NestedModel.ShortNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.ShortNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.ShortNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.ShortNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.ShortNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.ShortNullable", FilterTypes.eq, "-25")]
        [TestCase("NestedModel.ShortNullable", FilterTypes.eq, "0")]
        [TestCase("NestedModel.ShortNullable", FilterTypes.eq, short.MaxValue)]
        [TestCase("NestedModel.ShortNullable", FilterTypes.eq, short.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("UShort", FilterTypes.gt, "25")]
        [TestCase("UShort", FilterTypes.gte, "25")]
        [TestCase("UShort", FilterTypes.lt, "25")]
        [TestCase("UShort", FilterTypes.lte, "25")]
        [TestCase("UShort", FilterTypes.eq, "25")]
        [TestCase("UShort", FilterTypes.eq, "9999")]
        [TestCase("UShort", FilterTypes.eq, "0")]
        [TestCase("UShort", FilterTypes.eq, ushort.MaxValue)]
        [TestCase("UShort", FilterTypes.eq, ushort.MinValue)]
        [TestCase("NestedModel.UShort", FilterTypes.gt, "25")]
        [TestCase("NestedModel.UShort", FilterTypes.gte, "25")]
        [TestCase("NestedModel.UShort", FilterTypes.lt, "25")]
        [TestCase("NestedModel.UShort", FilterTypes.lte, "25")]
        [TestCase("NestedModel.UShort", FilterTypes.eq, "25")]
        [TestCase("NestedModel.UShort", FilterTypes.eq, "0")]
        [TestCase("NestedModel.UShort", FilterTypes.eq, ushort.MaxValue)]
        [TestCase("NestedModel.UShort", FilterTypes.eq, ushort.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("UShortNullable", FilterTypes.gt, "25")]
        [TestCase("UShortNullable", FilterTypes.gte, "25")]
        [TestCase("UShortNullable", FilterTypes.lt, "25")]
        [TestCase("UShortNullable", FilterTypes.lte, "25")]
        [TestCase("UShortNullable", FilterTypes.eq, "25")]
        [TestCase("UShortNullable", FilterTypes.eq, "9999")]
        [TestCase("UShortNullable", FilterTypes.eq, ushort.MaxValue)]
        [TestCase("UShortNullable", FilterTypes.eq, ushort.MinValue)]
        [TestCase("NestedModel.UShortNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.UShortNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.UShortNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.UShortNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.UShortNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.UShortNullable", FilterTypes.eq, "9999")]
        [TestCase("NestedModel.UShortNullable", FilterTypes.eq, ushort.MaxValue)]
        [TestCase("NestedModel.UShortNullable", FilterTypes.eq, ushort.MinValue)]
        // ---------------------------------------------------------------      
        [TestCase("ByteProperty", FilterTypes.gt, "25")]
        [TestCase("ByteProperty", FilterTypes.gte, "25")]
        [TestCase("ByteProperty", FilterTypes.lt, "25")]
        [TestCase("ByteProperty", FilterTypes.lte, "25")]
        [TestCase("ByteProperty", FilterTypes.eq, "25")]
        [TestCase("ByteProperty", FilterTypes.eq, "0")]
        [TestCase("ByteProperty", FilterTypes.eq, byte.MaxValue)]
        [TestCase("ByteProperty", FilterTypes.eq, byte.MinValue)]
        [TestCase("NestedModel.Byte", FilterTypes.gt, "25")]
        [TestCase("NestedModel.Byte", FilterTypes.gte, "25")]
        [TestCase("NestedModel.Byte", FilterTypes.lt, "25")]
        [TestCase("NestedModel.Byte", FilterTypes.lte, "25")]
        [TestCase("NestedModel.Byte", FilterTypes.eq, "25")]
        [TestCase("NestedModel.Byte", FilterTypes.eq, "0")]
        [TestCase("NestedModel.Byte", FilterTypes.eq, byte.MaxValue)]
        [TestCase("NestedModel.Byte", FilterTypes.eq, byte.MinValue)]
        // ---------------------------------------------------------------      
        [TestCase("ByteNullable", FilterTypes.gt, "25")]
        [TestCase("ByteNullable", FilterTypes.gte, "25")]
        [TestCase("ByteNullable", FilterTypes.lt, "25")]
        [TestCase("ByteNullable", FilterTypes.lte, "25")]
        [TestCase("ByteNullable", FilterTypes.eq, "25")]
        [TestCase("ByteNullable", FilterTypes.eq, "0")]
        [TestCase("ByteNullable", FilterTypes.eq, byte.MaxValue)]
        [TestCase("ByteNullable", FilterTypes.eq, byte.MinValue)]
        [TestCase("NestedModel.ByteNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.ByteNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.ByteNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.ByteNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.ByteNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.ByteNullable", FilterTypes.eq, "0")]
        [TestCase("NestedModel.ByteNullable", FilterTypes.eq, byte.MaxValue)]
        [TestCase("NestedModel.ByteNullable", FilterTypes.eq, byte.MinValue)]
        // ---------------------------------------------------------------      
        [TestCase("SByteProperty", FilterTypes.gt, "25")]
        [TestCase("SByteProperty", FilterTypes.gte, "25")]
        [TestCase("SByteProperty", FilterTypes.lt, "25")]
        [TestCase("SByteProperty", FilterTypes.lte, "25")]
        [TestCase("SByteProperty", FilterTypes.eq, "25")]
        [TestCase("SByteProperty", FilterTypes.eq, "-25")]
        [TestCase("SByteProperty", FilterTypes.eq, "0")]
        [TestCase("SByteProperty", FilterTypes.eq, sbyte.MaxValue)]
        [TestCase("SByteProperty", FilterTypes.eq, sbyte.MinValue)]
        [TestCase("NestedModel.SByteProperty", FilterTypes.gt, "25")]
        [TestCase("NestedModel.SByteProperty", FilterTypes.gte, "25")]
        [TestCase("NestedModel.SByteProperty", FilterTypes.lt, "25")]
        [TestCase("NestedModel.SByteProperty", FilterTypes.lte, "25")]
        [TestCase("NestedModel.SByteProperty", FilterTypes.eq, "25")]
        [TestCase("NestedModel.SByteProperty", FilterTypes.eq, "-25")]
        [TestCase("NestedModel.SByteProperty", FilterTypes.eq, "0")]
        [TestCase("NestedModel.SByteProperty", FilterTypes.eq, sbyte.MaxValue)]
        [TestCase("NestedModel.SByteProperty", FilterTypes.eq, sbyte.MinValue)]
        // ---------------------------------------------------------------      
        [TestCase("SByteNullable", FilterTypes.gt, "25")]
        [TestCase("SByteNullable", FilterTypes.gte, "25")]
        [TestCase("SByteNullable", FilterTypes.lt, "25")]
        [TestCase("SByteNullable", FilterTypes.lte, "25")]
        [TestCase("SByteNullable", FilterTypes.eq, "25")]
        [TestCase("SByteNullable", FilterTypes.eq, "-25")]
        [TestCase("SByteNullable", FilterTypes.eq, "0")]
        [TestCase("SByteNullable", FilterTypes.eq, sbyte.MaxValue)]
        [TestCase("SByteNullable", FilterTypes.eq, sbyte.MinValue)]
        [TestCase("NestedModel.SByteNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.SByteNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.SByteNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.SByteNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.SByteNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.SByteNullable", FilterTypes.eq, "-25")]
        [TestCase("NestedModel.SByteNullable", FilterTypes.eq, "0")]
        [TestCase("NestedModel.SByteNullable", FilterTypes.eq, sbyte.MaxValue)]
        [TestCase("NestedModel.SByteNullable", FilterTypes.eq, sbyte.MinValue)]
        // ---------------------------------------------------------------      
        [TestCase("DoubleProperty", FilterTypes.gt, "25.1543254325")]
        [TestCase("DoubleProperty", FilterTypes.gte, "25.1543254325")]
        [TestCase("DoubleProperty", FilterTypes.lt, "25.1543254325")]
        [TestCase("DoubleProperty", FilterTypes.lte, "25.1543254325")]
        [TestCase("DoubleProperty", FilterTypes.eq, "25.1543254325")]
        [TestCase("DoubleProperty", FilterTypes.eq, "-25.1543254325")]
        [TestCase("DoubleProperty", FilterTypes.eq, "0")]
        [TestCase("DoubleProperty", FilterTypes.eq, 1.7976931348623157)]
        [TestCase("DoubleProperty", FilterTypes.eq, -1.7976931348623157)]
        [TestCase("NestedModel.DoubleProperty", FilterTypes.gt, "25.1543254325")]
        [TestCase("NestedModel.DoubleProperty", FilterTypes.gte, "25.1543254325")]
        [TestCase("NestedModel.DoubleProperty", FilterTypes.lt, "25.1543254325")]
        [TestCase("NestedModel.DoubleProperty", FilterTypes.lte, "25.1543254325")]
        [TestCase("NestedModel.DoubleProperty", FilterTypes.eq, "25.1543254325")]
        [TestCase("NestedModel.DoubleProperty", FilterTypes.eq, "-25.1543254325")]
        [TestCase("NestedModel.DoubleProperty", FilterTypes.eq, "0")]
        [TestCase("NestedModel.DoubleProperty", FilterTypes.eq, 1.7976931348623157)]
        [TestCase("NestedModel.DoubleProperty", FilterTypes.eq, -1.7976931348623157)]
        // ---------------------------------------------------------------      
        [TestCase("DoubleNullable", FilterTypes.gt, "25.1543254325")]
        [TestCase("DoubleNullable", FilterTypes.gte, "25.1543254325")]
        [TestCase("DoubleNullable", FilterTypes.lt, "25.1543254325")]
        [TestCase("DoubleNullable", FilterTypes.lte, "25.1543254325")]
        [TestCase("DoubleNullable", FilterTypes.eq, "25.1543254325")]
        [TestCase("DoubleNullable", FilterTypes.eq, "-25.1543254325")]
        [TestCase("DoubleNullable", FilterTypes.eq, "0")]
        [TestCase("DoubleNullable", FilterTypes.eq, 1.7976931348623157)]
        [TestCase("DoubleNullable", FilterTypes.eq, -1.7976931348623157)]
        [TestCase("NestedModel.DoubleNullable", FilterTypes.gt, "25.1543254325")]
        [TestCase("NestedModel.DoubleNullable", FilterTypes.gte, "25.1543254325")]
        [TestCase("NestedModel.DoubleNullable", FilterTypes.lt, "25.1543254325")]
        [TestCase("NestedModel.DoubleNullable", FilterTypes.lte, "25.1543254325")]
        [TestCase("NestedModel.DoubleNullable", FilterTypes.eq, "25.1543254325")]
        [TestCase("NestedModel.DoubleNullable", FilterTypes.eq, "-25.1543254325")]
        [TestCase("NestedModel.DoubleNullable", FilterTypes.eq, "0")]
        [TestCase("NestedModel.DoubleNullable", FilterTypes.eq, 1.7976931348623157)]
        [TestCase("NestedModel.DoubleNullable", FilterTypes.eq, -1.7976931348623157)]
        // ---------------------------------------------------------------      
        [TestCase("DecimalProperty", FilterTypes.gt, "25.1543254325")]
        [TestCase("DecimalProperty", FilterTypes.gte, "25.1543254325")]
        [TestCase("DecimalProperty", FilterTypes.lt, "25.1543254325")]
        [TestCase("DecimalProperty", FilterTypes.lte, "25.1543254325")]
        [TestCase("DecimalProperty", FilterTypes.eq, "25.1543254325")]
        [TestCase("DecimalProperty", FilterTypes.eq, "-25.1543254325")]
        [TestCase("DecimalProperty", FilterTypes.eq, "0")]
        [TestCase("NestedModel.DecimalProperty", FilterTypes.gt, "25.1543254325")]
        [TestCase("NestedModel.DecimalProperty", FilterTypes.gte, "25.1543254325")]
        [TestCase("NestedModel.DecimalProperty", FilterTypes.lt, "25.1543254325")]
        [TestCase("NestedModel.DecimalProperty", FilterTypes.lte, "25.1543254325")]
        [TestCase("NestedModel.DecimalProperty", FilterTypes.eq, "25.1543254325")]
        [TestCase("NestedModel.DecimalProperty", FilterTypes.eq, "-25.1543254325")]
        [TestCase("NestedModel.DecimalProperty", FilterTypes.eq, "0")]
        // ---------------------------------------------------------------      
        [TestCase("DecimalNullable", FilterTypes.gt, "25.1543254325")]
        [TestCase("DecimalNullable", FilterTypes.gte, "25.1543254325")]
        [TestCase("DecimalNullable", FilterTypes.lt, "25.1543254325")]
        [TestCase("DecimalNullable", FilterTypes.lte, "25.1543254325")]
        [TestCase("DecimalNullable", FilterTypes.eq, "25.1543254325")]
        [TestCase("DecimalNullable", FilterTypes.eq, "-25.1543254325")]
        [TestCase("DecimalNullable", FilterTypes.eq, "0")]
        [TestCase("NestedModel.DecimalNullable", FilterTypes.gt, "25.1543254325")]
        [TestCase("NestedModel.DecimalNullable", FilterTypes.gte, "25.1543254325")]
        [TestCase("NestedModel.DecimalNullable", FilterTypes.lt, "25.1543254325")]
        [TestCase("NestedModel.DecimalNullable", FilterTypes.lte, "25.1543254325")]
        [TestCase("NestedModel.DecimalNullable", FilterTypes.eq, "25.1543254325")]
        [TestCase("NestedModel.DecimalNullable", FilterTypes.eq, "-25.1543254325")]
        [TestCase("NestedModel.DecimalNullable", FilterTypes.eq, "0")]
        public void CustomFilters_ShouldWorkProperlyForRangeWithAllSupportedTypes(string column, FilterTypes filterType, object value)
        {
            var requestModel = new RequestInfoModel
            {
                TableParameters = new DataTableAjaxPostModel
                {
                    Custom = new Custom
                    {
                        Filters = new Dictionary<string, IEnumerable<FilterModel>>
                        {
                            {
                                column, new List<FilterModel>
                                {
                                    new FilterModel { Type = filterType, Value = value.ToString() }
                                }
                            }
                        }
                    }
                }
            };

            var processedData = filter.ProcessData(data, requestModel);
            var predicate = $"{column} {this.GetFilterCsRepresentation(filterType)} {value.ToString()}";
            var expectedData = data.ToList().Where(predicate);

            Trace.WriteLine($"Number of results: {processedData.Count()}");
            Assert.AreEqual(processedData.Count(), expectedData.Count());
        }

        private string GetFilterCsRepresentation(FilterTypes filterType)
        {
            switch (filterType)
            {
                case FilterTypes.gte:
                    return ">=";

                case FilterTypes.gt:
                    return ">";

                case FilterTypes.lt:
                    return "<";

                case FilterTypes.lte:
                    return "<=";

                case FilterTypes.eq:
                    return "==";

                default:
                    throw new ArgumentException();
            }
        }
    }
}