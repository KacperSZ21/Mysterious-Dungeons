using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Behaviours/Berserk")]
public class BerserkSkill : SkillBehaviour
{
    public override void Cast(GameObject caster, SkillData data)
    {
        BerserkBuff buff = caster.GetComponent<BerserkBuff>();

        if (buff == null)
            return;

        buff.Activate(data.attackSpeed, data.buffDuration);

        if (data.particlePrefab != null)
        {
            Instantiate(data.particlePrefab, caster.transform.position, Quaternion.identity);
        }
    }
}