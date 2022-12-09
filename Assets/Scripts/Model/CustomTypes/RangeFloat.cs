using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Model.CustomTypes
{
    [Serializable]
    public struct RangeFloat
    {
        public static readonly RangeFloat Empty = new RangeFloat();
        public static readonly RangeFloat One = new RangeFloat
        {
            _minValue = 0f,
            _maxValue = 1f
        };

        [SerializeField]
        private float _minValue;
        [SerializeField]
        private float _maxValue;

        public float MinValue => _minValue;

        public float MaxValue => _maxValue;

        private readonly float Min => Mathf.Min(_minValue, _maxValue);

        private readonly float Max => Mathf.Max(_minValue, _maxValue);

        public readonly float GetRandom()
        {
            return Random.Range(Min, Max);
        }
    }
}