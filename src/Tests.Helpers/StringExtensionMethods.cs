namespace Tests.Helpers
{
    using System;
    using System.Text;
    using FakeData;

    public static class StringExtensionMethods
    {
        public static string RandomiseCase(this string str)
        {
            var random = new Random();
            var builder = new StringBuilder();
            foreach (var character in str)
            {
                var isUpperCase = BooleanData.GetBoolean();
                builder.Append(isUpperCase ? Char.ToUpper(character) : Char.ToLower(character));
            }

            return builder.ToString();
        }
    }
}