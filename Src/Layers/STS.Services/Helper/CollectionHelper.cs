using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Helper
{
    public static class CollectionHelper
    {
        public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            if (source != null)
                foreach (T item in source)
                {
                    destination.Add(item);
                }
        }


        public static void RemoveRange<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            if (source != null)
                foreach (T item in source)
                {
                    destination.Remove(item);
                }
        }
    }
}
