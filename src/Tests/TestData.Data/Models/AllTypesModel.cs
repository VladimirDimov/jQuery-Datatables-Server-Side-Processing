namespace TestData.Models
{
    using System;

    public class AllTypesModel
    {
        public int Id { get; set; }

        public string StringProperty { get; set; }

        public int Integer { get; set; }
        public int? IntegerNullable { get; set; }
#if USE_UTYPES
        public uint UInt { get; set; }
        public uint? UIntNullable { get; set; }
#else
        public int UInt { get; set; }
        public int? UIntNullable { get; set; }
#endif
        public long Long { get; set; }
        public long? LongNullable { get; set; }
#if USE_UTYPES
        public ulong ULong { get; set; }
        public ulong? ULongNullable { get; set; }
#else
        public long ULong { get; set; }
        public long? ULongNullable { get; set; }
#endif
        public short Short { get; set; }
        public short? ShortNullable { get; set; }
#if USE_UTYPES
        public ushort UShort { get; set; }
        public ushort? UShortNullable { get; set; }
#else
        public short UShort { get; set; }
        public short? UShortNullable { get; set; }
#endif
        public byte ByteProperty { get; set; }
        public byte? ByteNullable { get; set; }
#if USE_STYPES
        public sbyte SByteProperty { get; set; }
        public sbyte? SByteNullable { get; set; }
#else
        public byte SByteProperty { get; set; }
        public byte? SByteNullable { get; set; }
#endif
        public double DoubleProperty { get; set; }
        public double? DoubleNullable { get; set; }

        public decimal DecimalProperty { get; set; }
        public decimal? DecimalNullable { get; set; }

        public DateTime DateTimeProperty { get; set; }
        public DateTime? DateTimeNullable { get; set; }
        public DateTimeOffset DateTimeOffsetProperty { get; set; }
        public DateTimeOffset? DateTimeOffsetNullable { get; set; }

        public bool BooleanProperty { get; set; }
        public bool? BooleanNullable { get; set; }
#if USE_CHARTYPE
        public char CharProperty { get; set; }
        public char? CharNullable { get; set; }
#else
        public string CharProperty { get; set; }
        public string CharNullable { get; set; }
#endif
        public AllTypesModel NestedModel { get; set; }
    }
}