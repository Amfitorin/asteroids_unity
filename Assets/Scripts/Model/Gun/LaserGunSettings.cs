using System;
using UnityEngine;

namespace Model.Gun
{
    [Serializable]
    public class LaserGunSettings : GunSettings
    {
        [SerializeField]
        private int _maxCount;

        [SerializeField]
        private float _useDelay;

        [SerializeField]
        private float _time;
        
        public int MaxCount => _maxCount;

        public float UseDelay => _useDelay;

        public float Time => _time;
    }
}