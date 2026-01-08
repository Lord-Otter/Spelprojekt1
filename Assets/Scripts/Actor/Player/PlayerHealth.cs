using System.Collections;
using System.Collections.Generic;
using Spelprojekt1;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;


public class PlayerHealth : HealthHandler
{
    //[SerializeField] private Canvas gameOverScreen;
    private CameraTargetBehaviour cameraTarget;
    [SerializeField] private float takeDamageCooldown;
    private float takeDamageTimer;

    private Collider2D hurtBox;

    public event System.Action<int> OnHealthChanged;
    public UnityEvent OnPlayerDiedEvent;

    [Header("I-Frames Flash")]
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private float iFrameFlashAmount = 0.25f;
    [SerializeField] private Color firstFlashColor = Color.red;

    private Coroutine iFramesFlashCoroutine;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeMagnitude;

    [Header("Audio")]
    [SerializeField] [Range(0, 1)] private float hurtSoundsVolume;
    [SerializeField] [Range(0,2 )] private int hurtDuckingLevel = 0;
    [SerializeField] private List<AudioClip> hurtSounds;
    [SerializeField] [Range(0, 1)] private float deathSoundVolume;
    [SerializeField] [Range(0,2 )] private int deathDuckingLevel = 0;
    [SerializeField] private AudioClip deathSound;

    protected override void Awake()
    {
        base.Awake();
        cameraTarget = GetComponentInChildren<CameraTargetBehaviour>();
        hurtBox = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();

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
                StopIFramesFlashing();
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

        //takeDamageTimer = takeDamageCooldown;
        //canTakeDamage = false;
    }

    protected override void HandleDamage()
    {
        base.HandleDamage();

        // Update UI
        // Play damage effects. Screen, particles, sprite, animation, etc.

        // Shake Camera
        cameraTarget.Shake(amplitude: 0.2f, duration: 0.5f, frequency: 30f);

        // Slow down time
        TimeManager.Instance.StartTimeScaleRecovery("exp", 0.25f);

        // Play damage sound effects.
        SFXManager.instance.PlayRandomSFXClip(hurtSounds, transform, hurtSoundsVolume, hurtDuckingLevel);

        // Maybe do time scale effects. Maybe depending on the attack.

        // Make invulnerable for a few frames
        takeDamageTimer = takeDamageCooldown;
        canTakeDamage = false;

        // Play IFrames flashing
        if(iFramesFlashCoroutine != null)
            StopCoroutine(iFramesFlashCoroutine);
        
        iFramesFlashCoroutine = StartCoroutine(IFramesFlashing());
    }

    protected override void HandleDeath()
    {
        OnPlayerDiedEvent?.Invoke();
        // Play death animation / change to death sprite.

        // Play death sound effect.
        SFXManager.instance.PlaySFXClip(deathSound, transform, deathSoundVolume, deathDuckingLevel);

        // Disable hurt box
        hurtBox.enabled = false;

        // Shake Camera
        cameraTarget.Shake(amplitude: 0.5f, duration: 0.5f, frequency: 100f);

        // Slow down time
        TimeManager.Instance.StartTimeScaleRecovery("lin", 5f);

        // Play game over music
        // Disable player control

        // Display game over screen

        // Stop other game processes like enemies.
        // Stopping enemy AI, spawning and showing game over screen can be a function.
    }

    private IEnumerator IFramesFlashing()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_FlashColor", firstFlashColor);
            materials[i].SetFloat("_FlashAmount", iFrameFlashAmount);
        }

        yield return new WaitForSeconds(blinkInterval);

        for (int i = 0; i < materials.Length; i++)
            materials[i].SetColor("_FlashColor", Color.white);

        bool flashOn = false;

        while (!canTakeDamage)
        {
            float amount = flashOn ? iFrameFlashAmount : 0f;

            for (int i = 0; i < materials.Length; i++)
                materials[i].SetFloat("_FlashAmount", amount);

            flashOn = !flashOn;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void StopIFramesFlashing()
    {
        if (iFramesFlashCoroutine != null)
        {
            StopCoroutine(iFramesFlashCoroutine);
            iFramesFlashCoroutine = null;
        }

        for (int i = 0; i < materials.Length; i++)
            materials[i].SetFloat("_FlashAmount", 0f);
    }
}