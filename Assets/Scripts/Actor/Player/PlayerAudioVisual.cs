using System.Collections.Generic;
using Spelprojekt1;
using UnityEngine;

public class PlayerAudioVisual : MonoBehaviour
{
    private PlayerShooter playerShooter;
    private AimController aimController;
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
        playerShooter = GetComponent<PlayerShooter>();
        aimController = GetComponent<AimController>();

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
        Vector2 moveVel = rb.linearVelocity;
        bool isMoving = moveVel.magnitude > deadzone;
        bool isCharging = playerShooter.State == PlayerShooter.ShootState.Charging;

        string aimDir = GetDirectionFromAngle(aimController.aimAngle);
        string torsoAnim;

        if (isCharging)
        {
            torsoAnim = $"attack_{aimDir.ToLower()}_torso";
        }
        else if (isMoving)
        {
            torsoAnim = $"run_{aimDir.ToLower()}_torso";
        }
        else
        {
            torsoAnim = "idle";
        }

        PlayAnimation(torsoAnimator, torsoAnim);

        if (!isMoving)
        {
            legsRenderer.enabled = false;
            return;
        }

        legsRenderer.enabled = true;

        //string moveDir = GetDirectionFromVelocity(moveVel);
        //string legsAnim = $"run_{moveDir.ToLower()}_legs";

        //PlayAnimation(legsAnimator, legsAnim);

        //bool movingOpposite = IsMovingOppositeOfAim(moveVel, aimController.aimAngle);
        //legsAnimator.speed = movingOpposite ? -1f : 1f;

        /*
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
        PlayAnimation(legsAnimator, newAnimLegs);*/
    }



    private void PlayAnimation(Animator animator, string animName)
    {
        if (currentAnimation == animName)
            return;

        currentAnimation = animName;
        animator.Play(animName);
    }
    #endregion

    private string GetDirectionFromAngle(float angle)
    {
        angle = Mathf.Repeat(angle + 360f, 360f);

        if (angle >= 45f && angle < 135f) return "Up";
        if (angle >= 135f && angle < 225f) return "Left";
        if (angle >= 225f && angle < 315f) return "Down";
        return "Right";
    }
}