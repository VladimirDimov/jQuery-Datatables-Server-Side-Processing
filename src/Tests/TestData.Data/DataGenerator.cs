namespace TestData.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FakeData;
    using TestData.Models;

    public static class DataGenerator
    {
        public static IQueryable<AllTypesModel> GenerateSimpleData(uint numberOfItems, int minMax = 500)
        {
            var min = minMax * -1;
            var max = minMax;
            var data = new List<AllTypesModel>();
            var random = new Random();
            for (int i = 0; i < numberOfItems; i++)
            {
                var itemToAdd = GenerateSimpleModel(random, min, max);
                itemToAdd.NestedModel = GenerateSimpleModel(random, min, max);

                data.Add(itemToAdd);
            }

            return data.AsQueryable();
        }

        private static AllTypesModel GenerateSimpleModel(Random random, int min, int max)
        {
            var itemToAdd = new AllTypesModel
            {
                Long = NumberData.GetNumber(min, max),
                LongNullable = RandomiseNullable(NumberData.GetNumber(min, max)),
#if USE_UTYPES
                ULong = (ulong)NumberData.GetNumber(0, max),
                ULongNullable = RandomiseNullable((ulong)NumberData.GetNumber(0, max)),
#else
                ULong = (long)NumberData.GetNumber(0, max),
                ULongNullable = (long?)RandomiseNullable(NumberData.GetNumber(0, max)),
#endif
                Integer = random.Next(min, max),
                IntegerNullable = RandomiseNullable(FakeData.NumberData.GetNumber(min, max)),

#if USE_UTYPES
                UInt = (uint)NumberData.GetNumber(0, max),
                UIntNullable = (uint?)RandomiseNullable(NumberData.GetNumber(0, max)),
#else
                UInt = (int)NumberData.GetNumber(0, max),
                UIntNullable = (int?)RandomiseNullable(NumberData.GetNumber(0, max)),
#endif
                DoubleProperty = NumberData.GetDouble(),
                DoubleNullable = RandomiseNullable(NumberData.GetDouble()),

                DecimalProperty = (decimal)NumberData.GetDouble(),
                DecimalNullable = (decimal?)RandomiseNullable(NumberData.GetDouble()),

                Short = (short)NumberData.GetNumber(min, max),
                ShortNullable = (short?)(NumberData.GetNumber(min, max)),
#if USE_UTYPES
                UShort = (ushort)NumberData.GetNumber(0, max),
                UShortNullable = (ushort?)RandomiseNullable(NumberData.GetNumber(0, max)),
#else
                UShort = (short)NumberData.GetNumber(0, max),
                UShortNullable = (short?)RandomiseNullable(NumberData.GetNumber(0, max)),
#endif
                ByteProperty = RandomByte(),
                ByteNullable = RandomiseNullable(RandomByte()),
#if USE_STYPES
                SByteProperty = RandomSByte(),
                SByteNullable = RandomiseNullable(RandomSByte()),
#else
                SByteProperty = RandomByte(),
                SByteNullable = RandomiseNullable(RandomByte()),
#endif

#if USE_CHARTYPE
                CharProperty = FakeData.TextData.GetAlphabetical(1)[0],
                CharNullable = RandomiseNullable(FakeData.TextData.GetAlphabetical(1)[0]),
#else
                CharProperty = FakeData.TextData.GetAlphabetical(1)[0].ToString(),
                CharNullable = RandomiseNullable(TextData.GetAlphaNumeric(1), 0.3),
#endif

                BooleanProperty = BooleanData.GetBoolean(),
                BooleanNullable = RandomiseNullable(BooleanData.GetBoolean()),

                StringProperty = RandomiseNullable(TextData.GetAlphaNumeric(NumberData.GetNumber(0, 20)), 0.3),

                DateTimeProperty = RandomDate(max),
                DateTimeNullable = RandomiseNullable(RandomDate(max)),
#if USE_DTOFFSET
                DateTimeOffsetProperty = RandomDateTimeOffset(max),
                DateTimeOffsetNullable = RandomiseNullable(RandomDateTimeOffset(max))
#else
                DateTimeOffsetProperty = RandomDate(max),
                DateTimeOffsetNullable = RandomiseNullable(RandomDate(max)),
#endif
            };

            return itemToAdd;
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

        public static T RandomiseNullable<T>(T value, double nullFraction)
            where T : class
        {
            var rnd = NumberData.GetNumber(0, 100);
            if (rnd / 100d <= nullFraction)
            {
                return null;
            }

            return value;
        }
    }
}