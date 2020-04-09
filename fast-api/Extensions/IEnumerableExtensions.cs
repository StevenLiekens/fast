using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fast_api.Extensions
{
    public static class IEnumerableExtensions
    {
        public static List<List<T>> Chunks<T>(this IEnumerable<T> collection, int size)
        {
            var chunks = new List<List<T>>();
            var count = 0;
            var temp = new List<T>();

            foreach (var element in collection)
            {
                if (count++ == size)
                {
                    chunks.Add(temp);
                    temp = new List<T>();
                    count = 1;
                }
                temp.Add(element);
            }
            chunks.Add(temp);

            return chunks;
        }
    }
}
