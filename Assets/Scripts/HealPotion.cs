using UnityEngine;
using UnityEngine.UI;

public class HealPotion : MonoBehaviour
{
    [Header("References")]
    public ClickToMove2D player;

    [Header("Heal")]
    public int healAmount = 50;

    [Header("Cooldown")]
    public float cooldown = 5f;

    [Header("Key")]
    public KeyCode healKey = KeyCode.E;

    [Header("UI")]
    public Image cooldownFill;

    private float currentCooldown = 0f;


    void Update()
    {
        // cooldown timer
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;

            currentCooldown = Mathf.Max(currentCooldown, 0f);
        }

        UpdateCooldownUI();

        // keyboard input
        if (Input.GetKeyDown(healKey))
        {
            UsePotion();
        }
    }


    // UI button
    public void UsePotion()
    {
        // cooldown active
        if (currentCooldown > 0)
            return;

        if (player == null)
            return;

        int currentHealth = player.GetCurrentHealth();

        int maxHealth = player.GetMaxHealth();

        // already full hp
        if (currentHealth >= maxHealth)
            return;

        currentHealth += healAmount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        player.SetCurrentHealth(currentHealth);

        player.UpdateStatsUI();

        // start cooldown
        currentCooldown = cooldown;
    }


    void UpdateCooldownUI()
    {
        if (cooldownFill == null)
            return;

        if (currentCooldown <= 0)
        {
            cooldownFill.fillAmount = 0;
            return;
        }

        cooldownFill.fillAmount = currentCooldown / cooldown;
    }
}