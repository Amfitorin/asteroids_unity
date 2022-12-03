using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIController.Button
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ButtonRaycaster))]
    public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField]
        private bool _animate = true;

        [SerializeField]
        [Tooltip("Скейл при нажатии на кнопку")]
        private Vector2 _pointerDownScale;

        [SerializeField]
        [Tooltip("Время анимации скейла")]
        private float _scaleTime;
        
        [SerializeField]
        private Image _image;

        [SerializeField]
        private UnityEngine.UI.Button.ButtonClickedEvent _onClick = new();

        [SerializeField]
        private Transform _targetRect;

        [SerializeField]
        private bool _playSound = true;

        private Sequence _animateSequence;
        public void Reset()
        {
            _onClick.RemoveAllListeners();
            _onClick.AddListener(PlaySound);
        }


        private void OnEnable()
        {
            _onClick.AddListener(PlaySound);
            if (_animateSequence != null)
                PointUpAnimation();
        }

        private void OnDisable()
        {
            _onClick.RemoveListener(PlaySound);
            _animateSequence?.Kill();
            _animateSequence = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.LogError("pointer click");
            if (Input.touchCount > 1)
                return;
            _onClick?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.LogError("pointer down");
            if (Input.touchCount > 1)
                return;
            if (!_animate) return;
            if (_animateSequence != null)
            {
                _animateSequence.Kill();
                _animateSequence = null;
            }

            _targetRect.transform.localScale = Vector3.one;
            _animateSequence = DOTween.Sequence();
            _animateSequence.Join(_targetRect.transform.DOScale(_pointerDownScale,
                _scaleTime));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.LogError("pointer up");
            if (!_animate) return;
            PointUpAnimation();
        }

        private void PlaySound()
        {
            // var clickSound = GetStateSound();
            // SoundManager.Instance.Execute(clickSound, this);
        }

        private void PointUpAnimation()
        {
            _animateSequence.Kill();
            _animateSequence = DOTween.Sequence();
            _animateSequence.Join(_targetRect.transform.DOScale(Vector3.one, _scaleTime));
        }
        
        public void SetText(string text)
        {
            _label.text = text;
        }
    }
}