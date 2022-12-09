using System;
using Model.Configs.Enemy;
using Model.EnvObject;
using UnityEngine;

namespace Model.Enemy
{
    [Serializable]
    public class AsteroidSettings
    {
        [SerializeField]
        private AsteroidType _type;

        [SerializeField]
        private AsteroidConfigLink _config;

        public AsteroidType Type => _type;

        public AsteroidConfigLink Config => _config;
    }
}