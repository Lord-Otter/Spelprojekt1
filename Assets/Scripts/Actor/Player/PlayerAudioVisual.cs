using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioVisual : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer torsoRenderer;
    [SerializeField] private SpriteRenderer legsRenderer;

    [Header("Audio")]
    [SerializeField] [Range(0, 1)] private float footstepsVolume;
    [SerializeField] private List<AudioClip> footsteps;
    private AudioSource audioSource;
    private float footStepTimer;
    [SerializeField] private float footStepCooldown;

    [Header("Animation")]
    [SerializeField] private Animator torsoAnimator;
    [SerializeField] private Animator legsAnimator;
    private string currentAnimation;
    private string lastDirection = "Down";
    private const float deadzone = 0.1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        torsoRenderer = transform.Find("Visual").transform.Find("Torso").GetComponent<SpriteRenderer>();
        legsRenderer = transform.Find("Visual").transform.Find("Legs").GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        torsoAnimator = transform.Find("Visual").transform.Find("Torso").GetComponent<Animator>();
        legsAnimator = transform.Find("Visual").transform.Find("Legs").GetComponent<Animator>();
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
            PlayAnimation(torsoAnimator, "idle");
            legsRenderer.enabled = false;
            return;
        }

        legsRenderer.enabled = true;

        float absX = Mathf.Abs(vel.x);
        float absY = Mathf.Abs(vel.y);

        string newAnimTorso = "";
        string newAnimLegs = "";

        // --- PRIORITY 1: LEFT / RIGHT ---
        if (absX >= absY)  
        {
            if (vel.x > 0)
            {
                newAnimTorso = "run_right_torso";
                newAnimLegs = "run_right_legs";
                lastDirection = "Right";
            }
            else
            {
                newAnimTorso = "run_left_torso";
                newAnimLegs = "run_left_legs";
                lastDirection = "Left";
            }
        }
        // --- PRIORITY 2: UP / DOWN ---
        else
        {
            if (vel.y > 0)
            {
                newAnimTorso = "run_up_torso";
                newAnimLegs = "run_up_legs";
                lastDirection = "Up";
            }
            else
            {
                newAnimTorso = "run_down_torso";
                newAnimLegs = "run_down_legs";
                lastDirection = "Down";
            }
        }

        if(vel.magnitude < 0.1f)
        {
            PlayAnimation(torsoAnimator, "idle");
            legsRenderer.enabled = false;
        }

        PlayAnimation(torsoAnimator, newAnimTorso);
        PlayAnimation(legsAnimator, newAnimLegs);
    }

    private void PlayAnimation(Animator animator, string animName)
    {
        if (currentAnimation == animName)
            return;

        currentAnimation = animName;
        animator.Play(animName);
    }
    #endregion
}