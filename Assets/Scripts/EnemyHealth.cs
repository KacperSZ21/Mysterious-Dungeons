using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 50;
    public int armor = 0;
    public ArmorType armorType = ArmorType.Physical;
    public int MaxHealth => maxHealth;
    public string enemyName = "Enemy";
    public string GetName() => enemyName;
    [HideInInspector]
    public int CurrentHealth => currentHealth;
    private int currentHealth;
    private bool isDead = false;
    private Coroutine burnCoroutine;
    private const int burnDamage = 5;
    private const float burnInterval = 5f;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage, DamageType damageType)
    {
        if (isDead) return;

        int finalDamage = damage;

        switch (armorType)
        {
            // MAGIC ARMOR
            case ArmorType.Magical:
                if (damageType == DamageType.Physical || damageType == DamageType.Fire)
                {
                    finalDamage = Mathf.RoundToInt(damage * 0.9f);
                }

                else if (damageType == DamageType.Magical)
                {
                    float reduction = armor * 0.01f;

                    reduction = Mathf.Clamp(reduction, 0f, 0.8f);

                    finalDamage = Mathf.RoundToInt(damage * (1f - reduction));
                }

                break;

            // PHYSICAL ARMOR
            case ArmorType.Physical:
                if (damageType == DamageType.Physical || damageType == DamageType.Fire)
                {
                    float reduction = armor * 0.01f;

                    reduction = Mathf.Clamp(reduction, 0f, 0.8f);

                    finalDamage = Mathf.RoundToInt(damage * (1f - reduction));
                }

                break;
        }

        currentHealth -= finalDamage;
        animator.SetTrigger("3_Damaged");

        if (damageType == DamageType.Fire)
        {
            ApplyBurn();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        isDead = true;
        animator.SetTrigger("4_Death");

        EnemyLoot loot = gameObject.GetComponent<EnemyLoot>();

        if (loot != null)
        {
            loot.TryDropLoot();
        }

        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
        }

        Destroy(gameObject, 0.6f);
    }
    void ApplyBurn()
    {
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
        }

        burnCoroutine =
            StartCoroutine(BurnRoutine());
    }

    System.Collections.IEnumerator BurnRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(
                burnInterval
            );

            currentHealth -= burnDamage;

            if (currentHealth <= 0)
            {
                Die();
                yield break;
            }
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }
}