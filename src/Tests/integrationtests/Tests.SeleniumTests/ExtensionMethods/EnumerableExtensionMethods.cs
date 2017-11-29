using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.SeleniumTests.ExtensionMethods
{
    public static class EnumerableExtensionMethods
    {
        public static int FirstIndexWhere<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            var length = collection.Count();
            var index = 0;
            foreach (var item in collection)
            {
                if (predicate(item))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }
    }
}