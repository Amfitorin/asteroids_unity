using System;
using Model.Configs.Enemy;
using Model.Enemy;
using UnityEngine;

namespace Model.Level
{
    [Serializable]
    public class EnemySettings
    {
        [SerializeField]
        private AsteroidSettings[] _asteroids;
        
        [SerializeField]
        private NloConfigLink _nlo;

        public AsteroidSettings[] Asteroids => _asteroids;

        public NloConfigLink Nlo => _nlo;
    }
}