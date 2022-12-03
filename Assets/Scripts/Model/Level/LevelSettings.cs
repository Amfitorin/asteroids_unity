using System;
// ReSharper disable InconsistentNaming

namespace Model.Level
{
    [Serializable]
    public class LevelSettings
    {
        public int Level;

        public int BigEnemyCount;

        public int MediumEnemyCount;

        public int SmallEnemyCount;
    }
}