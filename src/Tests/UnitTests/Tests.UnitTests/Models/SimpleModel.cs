namespace Tests.UnitTests.Models
{
    using System;

    internal class SimpleModel
    {
        public string String { get; set; }

        public int Integer { get; set; }
        public int? IntegerNullable { get; set; }
        public uint UInt { get; set; }
        public uint? UIntNullable { get; set; }

        public long Long { get; set; }
        public long? LongNullable { get; set; }
        public ulong ULong { get; set; }
        public ulong? ULongNullable { get; set; }

        public short Short { get; set; }
        public short? ShortNullable { get; set; }
        public ushort UShort { get; set; }
        public ushort? UShortNullable { get; set; }

        public byte Byte { get; set; }
        public byte? ByteNullable { get; set; }
        public sbyte SByte { get; set; }
        public sbyte? SByteNullable { get; set; }

        public double Double { get; set; }
        public double? DoubleNullable { get; set; }

        public decimal Decimal { get; set; }
        public decimal? DecimalNullable { get; set; }

        public DateTime DateTime { get; set; }
        public DateTime? DateTimeNullable { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public DateTimeOffset? DateTimeOffsetNullable { get; set; }

        public bool Boolean { get; set; }
        public bool? BooleanNullable { get; set; }

        public char Char { get; set; }
        public char? CharNullable { get; set; }
    }
}