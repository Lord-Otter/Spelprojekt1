using Spelprojekt1;
using UnityEngine;

public class PlayerHealth : HealthHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
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
