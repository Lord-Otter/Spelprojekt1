using System.Collections.Generic;
using Spelprojekt1;
using UnityEngine;

public class PlayerAudioVisual : MonoBehaviour
{
    private PlayerShooter playerShooter;
    private AimController aimController;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private SpriteRenderer torsoRenderer;
    private SpriteRenderer legsRenderer;

    [Header("Audio")]
    [SerializeField] [Range(0, 1)] private float footstepsVolume;
    [SerializeField] private List<AudioClip> footsteps;
    private AudioSource audioSource;
    private float footStepTimer;
    [SerializeField] private float footStepCooldown;

    [Header("Animation")]
    private Animator torsoAnimator;
    private Animator legsAnimator;
    private string currentTorsoAnimation;
    private string currentLegsAnimation;
    private string lastDirection = "Down";
    private const float deadzone = 0.1f;

    void Awake()
    {
        playerShooter = GetComponent<PlayerShooter>();
        aimController = GetComponent<AimController>();
        playerMovement = GetComponent<PlayerMovement>();

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
        if (playerMovement.State == PlayerMovement.MoveState.Dash)
        {
            HandleDashAnimation();
            return;
        }

        Vector2 moveVel = rb.linearVelocity;
        bool isMoving = moveVel.magnitude > deadzone;
        bool isCharging = playerShooter.State == PlayerShooter.ShootState.Charging;

        string torsoDir;

        if (isCharging)
        {
            torsoDir = GetDirectionFromAngle(aimController.aimAngle);
        }
        else if (isMoving)
        {
            torsoDir = GetDirectionFromVelocity(moveVel);
        }
        else
        {
            torsoDir = lastDirection;
        }

        if (isMoving)
        {
            lastDirection = torsoDir;
        }

        //string aimDir = GetDirectionFromAngle(aimController.aimAngle);
        string torsoAnim;

        if (isCharging)
        {
            torsoAnim = $"attack_{torsoDir}_torso";
        }
        else if (isMoving)
        {
            torsoAnim = $"run_{torsoDir}_torso";
        }
        else
        {
            torsoAnim = "idle";
        }

        PlayTorsoAnimation(torsoAnim);

        if (!isMoving && !isCharging)
        {
            legsRenderer.enabled = false;
            return;
        }

        legsRenderer.enabled = true;

        legsAnimator.speed = isCharging ? 0.5f : 1f;

        if (isCharging && !isMoving)
        {
            string idleDir = GetDirectionFromAngle(aimController.aimAngle);
            PlayLegsAnimation($"idle_{idleDir}_legs");
            return;
        }

        string moveDir = GetDirectionFromVelocity(moveVel);

        if (isCharging)
        {
            bool movingOpposite = IsMovingOppositeOfAim(moveVel, aimController.aimAngle);

            string legsAnim = movingOpposite
                ? $"back_{moveDir}_legs"
                : $"run_{moveDir}_legs";

            PlayLegsAnimation(legsAnim);
        }
        else
        {
            PlayLegsAnimation($"run_{moveDir}_legs");
        }

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

    private void PlayTorsoAnimation(string animName)
    {
        if (currentTorsoAnimation == animName)
            return;

        currentTorsoAnimation = animName;
        torsoAnimator.Play(animName);
    }

    private void PlayLegsAnimation(string animName)
    {
        if (currentLegsAnimation == animName)
            return;

        currentLegsAnimation = animName;
        legsAnimator.Play(animName);
    }

    /*private void PlayAnimation(Animator animator, string animName)
    {
        if (currentAnimation == animName)
            return;

        currentAnimation = animName;
        animator.Play(animName);
    }*/
    #endregion

    private string GetDirectionFromAngle(float angle)
    {
        angle = Mathf.Repeat(angle + 360f, 360f);

        if (angle >= 45f && angle < 135f) return "up";
        if (angle >= 135f && angle < 225f) return "left";
        if (angle >= 225f && angle < 315f) return "down";
        return "right";
    }

    private string GetDirectionFromVelocity(Vector2 vel)
    {
        if (Mathf.Abs(vel.x) >= Mathf.Abs(vel.y))
            return vel.x >= 0 ? "right" : "left";
        else
            return vel.y >= 0 ? "up" : "down";
    }

    private bool IsMovingOppositeOfAim(Vector2 velocity, float aimAngle)
    {
        Vector2 aimDir = new Vector2(
            Mathf.Cos(aimAngle * Mathf.Deg2Rad),
            Mathf.Sin(aimAngle * Mathf.Deg2Rad)
        );

        velocity.Normalize();
        float dot = Vector2.Dot(aimDir, velocity);

        return dot < -0.1f; // moving against aim
    }

    private void HandleDashAnimation()
    {
        // Disable legs during dash
        legsRenderer.enabled = false;

        Vector2 dashDir = rb.linearVelocity;

        if (dashDir.sqrMagnitude < 0.01f)
            return;

        string dir = GetDirectionFromVelocity(dashDir);
        string dashAnim = $"dash_{dir}";

        if (currentTorsoAnimation == dashAnim)
            return;

        currentTorsoAnimation = dashAnim;
        torsoAnimator.Play(dashAnim);
    }
}