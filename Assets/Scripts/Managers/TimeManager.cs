using System.Collections;
using UnityEngine;

namespace Spelprojekt1
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        [Header("Debug")]
        [SerializeField] private float currentTimeScale = 1f;
        [SerializeField] private float targetTimeScale = 1f;

        private const float defaultTimeScale = 1f;
        private bool isPaused;
        private Coroutine timeScaleRecovery;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            //DontDestroyOnLoad(gameObject);

            targetTimeScale = defaultTimeScale;
            currentTimeScale = targetTimeScale;
        }

        private void Update()
        {
            Time.timeScale = currentTimeScale;
        }

        public void Pause()
        {
            isPaused = true;
            currentTimeScale = 0f;
        }

        public void Resume()
        {
            isPaused = false;
            currentTimeScale = targetTimeScale;
        }

        public void StartTimeScaleRecovery(string curve, float duration)
        {
            if (timeScaleRecovery != null)
                StopCoroutine(timeScaleRecovery);

            timeScaleRecovery = StartCoroutine(TimeScaleRecovery(curve, duration));
        }

        IEnumerator TimeScaleRecovery(string curve, float duration)
        {
            targetTimeScale = 0.01f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                if(isPaused)
                {
                    yield return null;
                    continue;
                }

                elapsedTime += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                float value = 0f;

                switch (curve)
                {
                    case "lin":
                        value = t;
                        break;

                    case "exp":
                        value = t * t;
                        break;

                    case "log":
                        value = Mathf.Log10(1 + 9 * t);
                        break;
                }

                targetTimeScale = Mathf.Clamp(value, 0f, 1f);

                if (!isPaused)
                    currentTimeScale = targetTimeScale;

                yield return null;
            }

            targetTimeScale = defaultTimeScale;

            if (!isPaused)
                currentTimeScale = targetTimeScale;
        }
    }
}