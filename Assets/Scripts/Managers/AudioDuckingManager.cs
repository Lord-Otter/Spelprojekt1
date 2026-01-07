using UnityEngine;
using UnityEngine.Audio;

public class AudioDuckingManager : MonoBehaviour
{
    public static AudioDuckingManager Instance { get; private set; }

    [SerializeField] private AudioMixerSnapshot normalSnapshot;
    [SerializeField] private AudioMixerSnapshot duckedSnapshot;

    [SerializeField] private float transitionTime;

    private int duckRequestCount = 0;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RequestDuck()
    {
        duckRequestCount++;

        if (duckRequestCount == 1)
        {
            duckedSnapshot.TransitionTo(transitionTime);
        }
    }

    public void ReleaseDuck()
    {
        duckRequestCount = Mathf.Max(0, duckRequestCount - 1);

        if(duckRequestCount == 0)
        {
            normalSnapshot.TransitionTo(transitionTime);
        }
    }
}
