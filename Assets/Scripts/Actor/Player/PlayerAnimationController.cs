using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private string currentAnimation;
    private string lastDirection = "Down"; // Default idle direction

    private const float deadzone = 1f; // Ignore tiny movement

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAnimation();
    }

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
}
