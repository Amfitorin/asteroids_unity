using Model.Configs.Enemy;
using UnityEngine;

namespace Gameplay.ViewApi.Enemy
{
    public class AsteroidSpawnData
    {
        public int ID { get; }
        public AsteroidConfig Config { get; }
        public Vector3 Direction { get; }

        public AsteroidSpawnData(int id, AsteroidConfig config, Vector3 direction)
        {
            ID = id;
            Config = config;
            Direction = direction;
        }
    }
}