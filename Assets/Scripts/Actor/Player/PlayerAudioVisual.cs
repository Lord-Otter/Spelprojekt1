using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioVisual : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Audio")]
    [SerializeField] private List<AudioClip> footsteps;
    private AudioSource audioSource;
    private float footStepTimer;
    [SerializeField] private float footStepCooldown;

    [Header("Animation")]
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        PlayFootSteps();
        AnimationParams();
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

    private void AnimationParams()
    {
        Vector2 velocity = rb.linearVelocity;

        animator.SetFloat("VelocityX", velocity.x);
        animator.SetFloat("VelocityY", velocity.y);
        animator.SetFloat("Speed", velocity.magnitude);
    }
}
