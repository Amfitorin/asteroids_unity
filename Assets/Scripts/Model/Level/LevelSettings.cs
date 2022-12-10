using System;
using UnityEngine;

namespace Model.Level
{
    [Serializable]
    public class LevelSettings
    {
        [SerializeField]
        private int _level;

        [SerializeField]
        private int _bigEnemyCount;

        [SerializeField]
        private int _mediumEnemyCount;

        [SerializeField]
        private int _smallEnemyCount;

        [SerializeField]
        private EnemySettings _enemies;

        public int Level => _level;

        public int BigEnemyCount => _bigEnemyCount;

        public int MediumEnemyCount => _mediumEnemyCount;

        public int SmallEnemyCount => _smallEnemyCount;

        public EnemySettings Enemies => _enemies;
    }
}