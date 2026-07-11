using UnityEngine;

public class AttackModifier
{
    public int bonusDamage;
    public DamageType? damageType;
    public bool guaranteedCrit;
    public GameObject particlePrefab;
}

public class PlayerAttackBuff : MonoBehaviour
{
    private int nextAttackBonusDamage;
    private DamageType? overrideDamageType;
    private bool guaranteedCrit;
    private GameObject nextAttackParticle;

    public void AddNextAttackBuff(int damage, GameObject particlePrefab)
    {
        nextAttackBonusDamage += damage;
        nextAttackParticle = particlePrefab;
    }

    public void SetNextAttackDamageType(DamageType type)
    {
        overrideDamageType = type;
    }

    public void SetGuaranteedCrit()
    {
        guaranteedCrit = true;
    }

    public AttackModifier Consume()
    {
        AttackModifier modifier = new()
        {
            bonusDamage = nextAttackBonusDamage,
            damageType = overrideDamageType,
            guaranteedCrit = guaranteedCrit,
            particlePrefab = nextAttackParticle
        };

        nextAttackBonusDamage = 0;
        overrideDamageType = null;
        guaranteedCrit = false;
        nextAttackParticle = null;

        return modifier;
    }
}