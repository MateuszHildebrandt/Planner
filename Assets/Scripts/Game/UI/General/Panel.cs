using State;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public enum PanelEffect { Alpha, Fade, Slide }

    [RequireComponent(typeof(CanvasGroup))]
    public class Panel : MonoBehaviour, IStateEnter, IStateExit
    {
        public bool IsActive { get; private set; } = true;

        [Header("Settings")]
        [SerializeField] PanelEffect userEffect;
        [SerializeField] bool centerOnAwake = true;
        [SerializeField] bool hideOnAwake = true;
        [SerializeField] float duration = 1;
        [SerializeField] float delay = 0;
        [SerializeField] bool useTimeScale = true;
        [SerializeField] bool setAsLastSibling = false;

        public enum MoveDirection { Up, Down, Left, Right, }
        public MoveDirection moveDirection;

        [Header("Events")]
        public UnityEvent onEnter;
        public UnityEvent onExit;

        //When effect completed ex. alpha, slide invoke this event and hide content GameObject.
        private System.Action onExitEffect;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

            if (hideOnAwake)
                SetPanelAlpha(false);
            else
                SetPanelAlpha(true);

            if (centerOnAwake)
                rectTransform.anchoredPosition = Vector3.zero;
        }

        private void SetPanel(bool value)
        {
            if (setAsLastSibling && value)
                transform.SetAsLastSibling();

            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;

            IsActive = value;

            if (IsActive)
                onEnter?.Invoke();
            else
                onExit?.Invoke();
        }       

        public void SetPanelAlpha(bool value)
        {
            if (IsActive == value)
                return;

            SetPanel(value);
            canvasGroup.alpha = (value) ? 1 : 0;
            onExitEffect?.Invoke();
        }

        public void SetPanelFade(bool value)
        {
            if (IsActive == value)
                return;

            SetPanel(value);
            StopAllCoroutines();
            StartCoroutine(AlphaCoroutine(value));
        }

        public void SetPanelSlide(bool value)
        {
            if (IsActive == value)
                return;

            SetPanel(value);
            StopAllCoroutines();
            StartCoroutine(SlideCoroutine(value));
        }

        public void SetUserEffect(bool value)
        {
            if (userEffect == PanelEffect.Alpha)
                SetPanelAlpha(value);
            else if (userEffect == PanelEffect.Fade)
                SetPanelFade(value);
            else if (userEffect == PanelEffect.Slide)
                SetPanelSlide(value);
        }

        [ExposeMethodInEditor]
        public void TogglePanelFade() => SetPanelFade(!IsActive);
        public void TogglePanelSlide() => SetPanelSlide(!IsActive);

        private IEnumerator AlphaCoroutine(bool isActive)
        {
            if (delay > 0)
            {
                if (useTimeScale)
                    yield return new WaitForSeconds(delay);
                else
                    yield return new WaitForSecondsRealtime(delay);
            }

            float start;
            float currValue = canvasGroup.alpha;

            if (isActive)
                start = (currValue < 1) ? currValue : 1;
            else
                start = (1 - currValue < 1) ? 1 - currValue : 1;

            for (float i = start; i < 1; i += (useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime) / duration)
            {
                canvasGroup.alpha = (isActive) ? i : 1 - i;
                yield return null;
            }

            canvasGroup.alpha = (isActive) ? 1 : 0;

            if (isActive == false)
                onExitEffect?.Invoke();
        }

        private IEnumerator SlideCoroutine(bool isActive)
        {
            if (delay > 0)
            {
                if (useTimeScale)
                    yield return new WaitForSeconds(delay);
                else
                    yield return new WaitForSecondsRealtime(delay);
            }

            Vector3 from, to;

            if (isActive)
            {
                canvasGroup.alpha = 1;
                from = SetSlideTarget();
                to = Vector3.zero;

                rectTransform.anchoredPosition = from;
            }
            else
            {
                from = Vector3.zero;
                to = SetSlideTarget();
            }

            for (float i = 0; i < 1; i += (useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime) / duration)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(from, to, i);
                yield return null;
            }

            rectTransform.anchoredPosition = to;
            if (isActive == false)
            {
                canvasGroup.alpha = 0;
                onExitEffect?.Invoke();
            }
        }

        private Vector3 SetSlideTarget()
        {
            Vector3 from = Vector3.zero;

            if (moveDirection == MoveDirection.Up)
                from += Vector3.down * rectTransform.rect.height;
            else if (moveDirection == MoveDirection.Down)
                from += Vector3.up * rectTransform.rect.height;
            else if (moveDirection == MoveDirection.Left)
                from += Vector3.right * rectTransform.rect.width;
            else if (moveDirection == MoveDirection.Right)
                from += Vector3.left * rectTransform.rect.width;

            return from;
        }

        #region StateMachine
        void IStateEnter.OnEnter()
        {
            SetUserEffect(true);
        }

        void IStateExit.OnExit()
        {
            SetUserEffect(false);
        }
        #endregion
    }
}
