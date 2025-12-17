using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioVisual : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Audio")]
    [SerializeField] [Range(0, 1)] private float footstepsVolume;
    [SerializeField] private List<AudioClip> footsteps;
    private AudioSource audioSource;
    private float footStepTimer;
    [SerializeField] private float footStepCooldown;

    [Header("Animation")]
    private Animator animator;
    private string currentAnimation;
    private string lastDirection = "Down";
    private const float deadzone = 0.1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        PlayFootSteps();
        HandleAnimation();
    }

    #region Audio
    private void PlayFootSteps()
    {
        if (Mathf.Abs(rb.linearVelocity.x) > 0.01f || Mathf.Abs(rb.linearVelocity.y) > 0.01f)
        {
            footStepTimer += Time.deltaTime;
            if(footStepTimer > footStepCooldown)
            {
                //PlaySFX(footsteps[Random.Range(0, 6)]);
                SFXManager.instance.PlayRandomSFXClip(footsteps, transform, footstepsVolume);

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
    #endregion

    #region Animation
    private void HandleAnimation()
    {
        Vector2 vel = rb.linearVelocity;
        
        // If idle
        if (vel.magnitude < deadzone)
        {
            PlayAnimation("Idle_Placeholder");
            return;
        }

        float absX = Mathf.Abs(vel.x);
        float absY = Mathf.Abs(vel.y);

        string newAnim = "";

        // --- PRIORITY 1: LEFT / RIGHT ---
        if (absX >= absY)  
        {
            if (vel.x > 0)
            {
                newAnim = "Run_Right";
                lastDirection = "Right";
            }
            else
            {
                newAnim = "Run_Left";
                lastDirection = "Left";
            }
        }
        // --- PRIORITY 2: UP / DOWN ---
        else
        {
            if (vel.y > 0)
            {
                newAnim = "Run_Up";
                lastDirection = "Up";
            }
            else
            {
                newAnim = "Run_Down";
                lastDirection = "Down";
            }
        }

        if(vel.magnitude < 0.1f)
        {
            PlayAnimation($"Idle_PlaceHolder");
        }

        PlayAnimation(newAnim);
    }

    private void PlayAnimation(string animName)
    {
        if (currentAnimation == animName)
            return;

        currentAnimation = animName;
        animator.Play(animName);
    }
    #endregion
}