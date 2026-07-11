using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Behaviours/Next Attack Buff")]
public class PowerStrikeSkill : SkillBehaviour
{
    public override void Cast(GameObject caster, SkillData data)
    {
        PlayerAttackBuff buff = caster.GetComponent<PlayerAttackBuff>();

        if (buff == null)
            return;

        buff.AddNextAttackBuff(data.bonusDamage, data.particlePrefab);
    }
}