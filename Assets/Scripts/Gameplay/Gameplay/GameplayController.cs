using Gameplay.ViewApi.Gameplay;
using Model.EnvObject;
using Model.Level;
using UnityEngine;

namespace Gameplay.Gameplay
{
    public class GameplayController : MonoBehaviour, IGameplayController
    {
        [SerializeField]
        private Transform _enemyRoot;

        [SerializeField]
        private Transform _playerRoot;

        [SerializeField]
        private LevelSettings[] _levels;

        [SerializeField]
        private PrefabSettings _prefabs;

        public void LoadPlayer()
        {
            
        }

        public void LoadNlo()
        {
            
        }

        public void LoadAsteroid(AsteroidType type)
        {
            
        }
    }
}