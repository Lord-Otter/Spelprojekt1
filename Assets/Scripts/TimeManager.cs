using System.Collections;
using UnityEngine;

namespace Spelprojekt1
{
    public class TimeManager : MonoBehaviour
    {
        #region Declarations
        [Header("Custom Time Pausing")]
        public GameObject trainingProjectile;
        //public ProjectileVelocity projectileVelocity;
        //public PlayerVelocity playerVelocity;
        //public CameraBehaviour cameraBehaviour;
        public bool worldPause = false;
        public bool tpPause = false;

        [Header("Time Scale")]
        public static float timeScale = 1.0f;
        public float unityTimeScale = 1.0f;
        [HideInInspector]public float lastUnityTimeScale;
        [HideInInspector]public Coroutine timeScaleRecovery;

        //[Header("Time Scale Recovery")]
        //private float duration = 5f;
        //private float elapsedTime = 0f;
        #endregion



        #region Unity Functions
        private void Awake()
        {
            //projectileVelocity = trainingProjectile.GetComponent<ProjectileVelocity>();
            //playerVelocity = GameObject.Find("Player").GetComponent<PlayerVelocity>();
            //cameraBehaviour = GameObject.Find("Main Camera").GetComponent<CameraBehaviour>();
        }

        // Update is called once per frame
        void Update()
        {
            CustomTimeScaler();
            UnityTimeScale();
        }

        void FixedUpdate()
        {
            
        }
        #endregion



        #region Time Scale
        void CustomTimeScaler()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(!worldPause)
                {
                    WorldPause(true);
                }
                else
                {
                    WorldPause(false);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                timeScale = 0.1f;
                TimeScaleChangeUpdate();
                Debug.Log($"Custom time Scale: {timeScale * 100}% ");
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                timeScale = 0.5f;
                TimeScaleChangeUpdate();
                Debug.Log($"Custom time Scale: {timeScale * 100}% ");
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                timeScale = 1f;
                TimeScaleChangeUpdate();
                Debug.Log($"Custom time Scale: {timeScale * 100}% ");
            }

        }

        public void StartTimeScaleRecovery(string curve, float duration)
        {
            timeScaleRecovery = StartCoroutine(TimeScaleRecovery(curve, duration));
        }

        public void StopTimeScaleRecovery()
        {
            if(timeScaleRecovery != null)
            {
                StopCoroutine(timeScaleRecovery);
            }
        }

        IEnumerator TimeScaleRecovery(string curve, float duration)
        {
            timeScale = 0.01f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime; // use unscaled time since timeScale may be 0
                float t = Mathf.Clamp01(elapsedTime / duration);
                float value = 0f;

                if (curve == "lin") // Linear
                {
                    value = t;
                    Debug.Log("Elapsed Time: " + elapsedTime);
                }
                else if (curve == "exp") // Exponential
                {
                    value = t * t;
                    Debug.Log("Elapsed Time: " + elapsedTime);
                }
                else if (curve == "log") // Logarithmic
                {
                    value = Mathf.Log10(1 + 9 * t);
                    Debug.Log("Elapsed Time: " + elapsedTime);
                }

                timeScale = value;
                TimeScaleChangeUpdate();
                Debug.Log("Time Scale: " + timeScale);
                yield return null;
            }

            timeScale = 1f;
            TimeScaleChangeUpdate();
        }

        public void WorldPause(bool pause)
        {
            if(pause)
            {
                Debug.Log("World Pause");
                worldPause = true;
            }
            else
            {
                Debug.Log("World Unpause");
                worldPause = false;

                //projectileVelocity.OnResume();
                //playerVelocity.OnResume();
            }
        }

        public void TPPause(bool pause)
        {
            if (pause)
            {
                Debug.Log("TP Pause");
                tpPause = true;                       
            }
            else
            {
                Debug.Log("TP Unpause");
                tpPause = false;

                //projectileVelocity.OnResume();
                //playerVelocity.OnResume();
            }        
        }

        void TimeScaleChangeUpdate()
        {
            //playerVelocity.AdjustVelocityForTimeScale();
            //projectileVelocity.AdjustVelocityForTimeScale();
            //cameraBehaviour.AdjustVelocityForTimeScale();
        }
        #endregion



        #region Unity Time Scale
        void UnityTimeScale() // Uses Unity's built in time scaler
        {
            Time.timeScale = unityTimeScale;

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                if (unityTimeScale == 0)
                {
                    unityTimeScale = lastUnityTimeScale;
                    Debug.Log($"Unity Time Scale: {timeScale * 100}% ");
                }
                else
                {
                    lastUnityTimeScale = timeScale;
                    unityTimeScale = 0;
                    Debug.Log("Stopping Unity Time");
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                unityTimeScale = 0.1f;
                Debug.Log($"Unity Time Scale: {unityTimeScale * 100}% ");
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                unityTimeScale = 0.5f;
                Debug.Log($"Unity Time Scale: {unityTimeScale * 100}% ");
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                unityTimeScale = 1;
                Debug.Log($"Unity Time Scale: {unityTimeScale * 100}% ");
            }
        }
        #endregion
    }
}