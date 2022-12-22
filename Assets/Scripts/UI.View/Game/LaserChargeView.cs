using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Game
{
    [Serializable]
    public class LaserChargeView
    {
        [SerializeField]
        private Image _state;

        [SerializeField]
        private Color _activeColor;

        [SerializeField]
        private Color _disableColor;

        [SerializeField]
        private Image _progress;

        [SerializeField]
        private TextMeshProUGUI _countCharges;

        [SerializeField]
        private TextMeshProUGUI _cooldownProgress;

        [SerializeField]
        private TextMeshProUGUI _useTime;

        [SerializeField]
        private GameObject _cooldownRoot;

        public void SetupProgress(float time, float totalTime)
        {
            if (!_cooldownRoot.activeSelf)
            {
                _cooldownRoot.SetActive(true);
            }

            var percent = time / totalTime;
            _progress.fillAmount = percent;
            _cooldownProgress.text = GetTimeString(totalTime - time);
        }

        public void FullCharges()
        {
            _cooldownRoot.SetActive(false);
        }

        public void SetCharges(int charges)
        {
            _countCharges.text = charges.ToString(CultureInfo.InvariantCulture);
            _state.color = charges > 0 ? _activeColor : _disableColor;
        }

        public void TimeUse(float time)
        {
            if (time <= 0f)
            {
                _useTime.gameObject.SetActive(false);
                return;
            }

            if (!_useTime.gameObject.activeSelf)
            {
                _useTime.gameObject.SetActive(true);
            }

            _useTime.text = GetTimeString(time);
        }

        private string GetTimeString(float time)
        {
            var span = TimeSpan.FromSeconds(time);

            return span.Seconds > 0 ? $"{span.Seconds}s {span.Milliseconds}ms" : $"{span.Milliseconds}ms";
        }

        public void CancelUse()
        {
            _useTime.gameObject.SetActive(false);
        }
    }
}