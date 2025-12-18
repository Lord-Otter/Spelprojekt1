using System.Collections.Generic;
using Spelprojekt1;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;


public class PlayerHealth : HealthHandler
{
    private CinemachineImpulseSource damageImpulse;

    [SerializeField] private float takeDamageCooldown;
    private float takeDamageTimer;

    private Collider2D hurtBox;

    public event System.Action<int> OnHealthChanged;
    public UnityEvent OnPlayerDiedEvent;

    [Header("Audio")]
    [SerializeField] [Range(0, 1)] private float hurtSoundsVolume;
    [SerializeField] private List<AudioClip> hurtSounds;
    [SerializeField] [Range(0, 1)] private float deathSoundVolume;
    [SerializeField] private AudioClip deathSound;

    protected override void Awake()
    {
        base.Awake();
        hurtBox = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();
        damageImpulse = GameObject.Find("PlayerDamageImpulse").GetComponent<CinemachineImpulseSource>();

        canTakeDamage = true;
        
        GetData();

        hurtBox.enabled = true;
    }

    private void GetData()
    {
        maxHealth = PlayerData.Instance.stats.maxHealth;
        currentHealth = PlayerData.Instance.stats.currentHealth;
    }

    protected override void Update()
    {
        if (!canTakeDamage)
        {
            takeDamageTimer -= Time.deltaTime;
            if(takeDamageTimer <= 0)
            {
                canTakeDamage = true;
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        if(!canTakeDamage)
        {   
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        PlayerData.Instance.stats.currentHealth = currentHealth; // Save health to pass along in new scenes

        OnHealthChanged?.Invoke(currentHealth);

        if(currentHealth > 0)
        {
            HandleDamage(); // Maybe make this a coroutine for a sequence of events.
        }
        else
        {
            HandleDeath(); // Maybe make this a coroutine for a sequence of events.
        }

        takeDamageTimer = takeDamageCooldown;
        canTakeDamage = false;



        // Update UI. If this doesn't work then use code below and 

        /*if(!canTakeDamage)
                return;

        currentHealth -= damage;

        // Update UI code here

        if(currentHealth > 0)
        {
            HandleDamage(); // Maybe make this a coroutine for a sequence of events.
        }
        else
        {
            HandleDeath(); // Maybe make this a coroutine for a sequence of events.
        }*/
    }

    protected override void HandleDamage()
    {
        // Update UI
        // Play damage effects. Screen, particles, sprite, animation, etc.
        damageImpulse.GenerateImpulse();

        // Play damage sound effects.
        SFXManager.instance.PlayRandomSFXClip(hurtSounds, transform, hurtSoundsVolume);

        // Maybe do time scale effects. Maybe depending on the attack.
        // Make invulnerable for a few frames
    }

    protected override void HandleDeath()
    {
        OnPlayerDiedEvent?.Invoke();
        // Play death animation / change to death sprite.

        // Play death sound effect.
        SFXManager.instance.PlaySFXClip(deathSound, transform, deathSoundVolume);

        // Disable hurt box
        hurtBox.enabled = false;

        // Play game over music
        // Do some time scale effects.
        // Disable player control
        // Display game over screen
        // Stop other game processes like enemies.
        // Stopping enemy AI, spawning and showing game over screen can be a function.
    }

    /*private void TriggerDamageShake()
    {
        if (damageImpulse == null)
            return;

        // Direction: push camera AWAY from hit
        Vector2 hitDirection = Vector2.zero;

        // Example: enemy hit the player
        // Replace with actual attacker position if available
        if (lastDamageSource != null)
        {
            hitDirection = (transform.position - lastDamageSource.position).normalized;
        }
        else
        {
            hitDirection = Random.insideUnitCircle.normalized;
        }

        // Impulse wants Vector3
        Vector3 impulseDir = new Vector3(hitDirection.x, hitDirection.y, 0f);

        damageImpulse.GenerateImpulse(impulseDir);
    }*/
}