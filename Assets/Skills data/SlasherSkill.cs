using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Behaviours/Slasher")]
public class SlasherSkill : SkillBehaviour
{
    public LayerMask enemyLayer;

    public override void Cast(GameObject caster, SkillData data)
    {
        if (data.particlePrefab != null)
        {
            Instantiate(data.particlePrefab, caster.transform.position, Quaternion.identity);
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(caster.transform.position, data.radius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            EnemyHealth hp = enemy.GetComponent<EnemyHealth>();

            if (hp == null)
                continue;

            hp.TakeDamage(data.damage, data.damageType);
        }
    }
}
