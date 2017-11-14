namespace Tests.SeleniumTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Helpers
    {
        public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            var index = 0;
            foreach (var item in collection)
            {
                if (predicate(item)) return index;

                index++;
            }

            return -1;
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> collection, int page, int size)
        {
            return collection
                .Skip((page - 1) * size)
                .Take(size);
        }
    }
}