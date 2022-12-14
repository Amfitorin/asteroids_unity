using UnityEngine;

namespace Core.Utils.Extensions
{
    public static class FloatExtensions
    {
        public static int AsMS(this float second)
        {
            return Mathf.RoundToInt(second * 1000);
        }
    }
}