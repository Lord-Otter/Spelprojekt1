using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    [Header("Vitals")]
    public int baseMaxHealth = 6;
    public int maxHealth = 6;
    public int currentHealth = 6;

    [Header("Modifiers")]
    public float moveSpeedMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float chargeSpeedMultiplier = 1f;

    public void Reset()
    {
        maxHealth = baseMaxHealth;
        currentHealth = maxHealth;
        moveSpeedMultiplier = 1f;
        damageMultiplier = 1f;
        chargeSpeedMultiplier = 1f;
    }
}