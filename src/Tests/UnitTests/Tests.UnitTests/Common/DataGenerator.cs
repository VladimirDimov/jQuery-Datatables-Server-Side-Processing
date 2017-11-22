using System;
using System.Collections.Generic;
using System.Linq;
using FakeData;
using Tests.UnitTests.Models;

namespace Tests.UnitTests.Common
{
    public static class DataGenerator
    {
        internal static IQueryable<SimpleModel> GenerateSimpleData(uint numberOfItems, int minMax = 500)
        {
            var min = minMax * -1;
            var max = minMax;
            var data = new List<SimpleModel>();
            var random = new Random();
            for (int i = 0; i < numberOfItems; i++)
            {
                var itemToAdd = new SimpleModel
                {
                    Long = NumberData.GetNumber(min, max),
                    LongNullable = RandomiseNullable(NumberData.GetNumber(min, max)),
                    ULong = (ulong)NumberData.GetNumber(0, max),
                    ULongNullable = RandomiseNullable((ulong)NumberData.GetNumber(0, max)),

                    Integer = random.Next(min, max),
                    IntegerNullable = RandomiseNullable(FakeData.NumberData.GetNumber(min, max)),
                    UInt = (uint)NumberData.GetNumber(0, max),
                    UIntNullable = (uint?)RandomiseNullable(NumberData.GetNumber(0, max)),

                    Double = NumberData.GetDouble(),
                    DoubleNullable = RandomiseNullable(NumberData.GetDouble()),

                    Short = (short)NumberData.GetNumber(min, max),
                    ShortNullable = (short?)(NumberData.GetNumber(min, max)),
                    UShort = (ushort)NumberData.GetNumber(0, max),
                    UShortNullable = (ushort?)RandomiseNullable(NumberData.GetNumber(0, max)),

                    Byte = RandomByte(),
                    ByteNullable = RandomiseNullable(RandomByte()),
                    SByte = RandomSByte(),
                    SByteNullable = RandomiseNullable(RandomSByte()),

                    Char = FakeData.TextData.GetAlphabetical(1)[0],
                    CharNullable = RandomiseNullable(FakeData.TextData.GetAlphabetical(1)[0]),

                    Boolean = BooleanData.GetBoolean(),
                    BooleanNullable = RandomiseNullable(BooleanData.GetBoolean()),

                    String = TextData.GetAlphaNumeric(NumberData.GetNumber(0, 20)),

                    DateTime = RandomDate(max),
                    DateTimeNullable = RandomiseNullable(RandomDate(max)),
                    DateTimeOffset = RandomDateTimeOffset(max),
                    DateTimeOffsetNullable = RandomiseNullable(RandomDateTimeOffset(max))
                };

                data.Add(itemToAdd);
            }

            return data.AsQueryable();
        }

        private static DateTimeOffset RandomDateTimeOffset(int max)
        {
            return new DateTimeOffset(RandomDate(max));
        }

        private static DateTime RandomDate(int max)
        {
            return DateTime.Now.AddDays(NumberData.GetNumber(max * (-1), 0));
        }

        private static sbyte RandomSByte()
        {
            return (sbyte)NumberData.GetNumber(sbyte.MinValue, sbyte.MaxValue);
        }

        private static byte RandomByte()
        {
            return (byte)NumberData.GetNumber(byte.MinValue, byte.MaxValue);
        }

        public static Nullable<T> RandomiseNullable<T>(T value)
            where T : struct
        {
            var isNull = FakeData.BooleanData.GetBoolean();

            return isNull ? new Nullable<T>() : new Nullable<T>(value);
        }
    }
}