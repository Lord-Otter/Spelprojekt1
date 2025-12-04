using Spelprojekt1;
using UnityEngine;
using UnityEngine.Events;


public class PlayerHealth : HealthHandler
{
    public event System.Action<int> OnHealthChanged;
    public UnityEvent OnPlayerDiedEvent;

    protected override void Start()
    {
        base.Start();
        canTakeDamage = true;
    }

    protected override void Update()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        if(!canTakeDamage)
        {   
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth);

        if(currentHealth > 0)
        {
            HandleDamage(); // Maybe make this a coroutine for a sequence of events.
        }
        else
        {
            HandleDeath(); // Maybe make this a coroutine for a sequence of events.
        }



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
        // Maybe do time scale effects. Maybe depending on the attack.
        // Make invulnerable for a few frames
    }

    protected override void HandleDeath()
    {
        OnPlayerDiedEvent?.Invoke();
        // Play death animation / change to death sprite.
        // Play death sound effect.
        // Play game over music
        // Do some time scale effects.
        // Disable player control
        // Display game over screen
        // Stop other game processes like enemies.
        // Stopping enemy AI, spawning and showing game over screen can be a function.
    }
}