using System.Collections.Generic;

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
    }
}