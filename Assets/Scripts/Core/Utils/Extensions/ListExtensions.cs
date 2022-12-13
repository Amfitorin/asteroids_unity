using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils.Extensions
{
    public static class ListExtensions
    {
        public static void RemoveWithReplaceLast<T>(this IList<T> list, T item)
        {
            var index = list.IndexOf(item);
            if (index < 0) return;
            var last = list.Count - 1;
            if (index != last)
                list[index] = list[last];
            list.RemoveAt(last);
        }

        public static T RandomElement<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }
        
        public static T RandomElement<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}