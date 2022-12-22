using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI.View.Game
{
    [Serializable]
    public class PlayerInfo
    {
        [SerializeField]
        private LifeView _lifes;

        [SerializeField]
        private TextMeshProUGUI _position;

        [SerializeField]
        private TextMeshProUGUI _rotation;

        [SerializeField]
        private TextMeshProUGUI _speed;

        public void UpdatePosition(Transform player, float speed)
        {
            var position = player.position;
            _position.text = $"x:{position.x:F}, y:{position.y:F}";
            var rotation = player.eulerAngles;
            _rotation.text = $"{rotation.z:F}";

            _speed.text = $"{speed:F}";
        }

        public void UpdateLifes(int lifes)
        {
            _lifes.Update(lifes);
        }

        public void Init()
        {
        }
    }
}