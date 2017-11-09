using System;
using System.Collections.Generic;
using System.Linq;
using FakeData;
using JQDT.Tests.Mocks.DataModels;

namespace JQDT.Tests.Mocks
{
    internal class DataGenerator
    {
        public IQueryable<SimpleDataModel> GenerateSimpleData(int numberOfRecords, DateTime minDate, DateTime maxDate)
        {
            var data = new List<SimpleDataModel>();
            var random = new Random();
            for (int i = 0; i < numberOfRecords; i++)
            {
                data.Add(new SimpleDataModel
                {
                    String = TextData.GetAlphabetical(random.Next(3, 20)),
                    Integer = NumberData.GetNumber(-5000, 5000),
                    Double = NumberData.GetDouble(),
                    Boolean = BooleanData.GetBoolean(),
                    DateTime = DateTimeData.GetDatetime(minDate, maxDate)
                });
            }

            return data.AsQueryable();
        }
    }
}