using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI.View.Game
{
    [Serializable]
    public class LevelInfo
    {
        [SerializeField]
        private TextMeshProUGUI _level;

        [SerializeField]
        private TextMeshProUGUI _bigCount;

        [SerializeField]
        private TextMeshProUGUI _mediumCount;

        [SerializeField]
        private TextMeshProUGUI _smallCount;

        [SerializeField]
        private TextMeshProUGUI _nlo;

        public void SetupLevel(int level)
        {
            _level.text = level.ToString(CultureInfo.InvariantCulture);
        }

        public void SetupAsteroids(int big, int med, int small)
        {
            _bigCount.text = big.ToString(CultureInfo.InvariantCulture);
            _mediumCount.text = med.ToString(CultureInfo.InvariantCulture);
            _smallCount.text = small.ToString(CultureInfo.InvariantCulture);
        }

        public void SetupNlo(int count)
        {
            _nlo.text = count.ToString(CultureInfo.InvariantCulture);
        }
    }
}