using System.Collections.Generic;
using Spelprojekt1;
using UnityEngine;
using UnityEngine.Events;


public class PlayerHealth : HealthHandler
{
    [SerializeField] private float takeDamageCooldown;
    private float takeDamageTimer;

    public event System.Action<int> OnHealthChanged;
    public UnityEvent OnPlayerDiedEvent;

    [Header("Audio")]
    [SerializeField] private List<AudioClip> hurtSounds;
    [SerializeField] private AudioClip deathSound;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        canTakeDamage = true;
        
        GetData();
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

        // Play damage sound effects.
        SFXManager.instance.PlayRandomSFXClip(hurtSounds, transform, 1);

        // Maybe do time scale effects. Maybe depending on the attack.
        // Make invulnerable for a few frames
    }

    protected override void HandleDeath()
    {
        OnPlayerDiedEvent?.Invoke();
        // Play death animation / change to death sprite.

        // Play death sound effect.
        SFXManager.instance.PlaySFXClip(deathSound, transform, 1);

        // Play game over music
        // Do some time scale effects.
        // Disable player control
        // Display game over screen
        // Stop other game processes like enemies.
        // Stopping enemy AI, spawning and showing game over screen can be a function.
    }
}