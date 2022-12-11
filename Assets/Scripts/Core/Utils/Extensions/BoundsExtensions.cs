using UnityEngine;

namespace Core.Utils.Extensions
{
    public static class BoundsExtensions
    {
        public static Rect AsRect(this Bounds bounds)
        {
            var rect = new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);
            return rect;
        }
    }
}