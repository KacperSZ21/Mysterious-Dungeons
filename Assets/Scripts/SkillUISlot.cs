using UnityEngine;
using UnityEngine.UI;

public class SkillUISlot : MonoBehaviour
{
    public Image icon;
    public Image cooldownImage;

    public void SetSkill(SkillData skill)
    {
        if (skill == null)
        {
            icon.enabled = false;
            cooldownImage.fillAmount = 0;
            return;
        }

        icon.enabled = true;
        icon.sprite = skill.icon;
        icon.preserveAspect = true;
    }

    public void SetCooldown(float value)
    {
        cooldownImage.fillAmount = value;
    }
}