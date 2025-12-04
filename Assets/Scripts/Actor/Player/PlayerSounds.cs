using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private List<AudioClip> footsteps;
    private AudioClip Attack;
    private AudioSource audioSource;
    private float footStepTimer;
    [SerializeField] private float footStepCooldown;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        PlayFootSteps();
    }

    private void PlayFootSteps()
    {
        if (Mathf.Abs(rb.linearVelocity.x) > 0.01f || Mathf.Abs(rb.linearVelocity.y) > 0.01f)
        {
            footStepTimer += Time.deltaTime;
            if(footStepTimer > footStepCooldown)
            {
                PlaySFX(footsteps[Random.Range(0, 6)]);

                footStepTimer = 0;
            }
        }
        else
        {
            footStepTimer = footStepCooldown / 2f;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
