using UnityEngine;

public class SkillUIController : MonoBehaviour
{
    [SerializeField] private PlayerSkills playerSkills;

    [SerializeField] private SkillUISlot[] uiSlots;

    void Start()
    {
        RefreshIcons();
    }

    void Update()
    {
        RefreshCooldowns();
    }

    void RefreshIcons()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (i >= playerSkills.skills.Length)
                break;

            uiSlots[i].SetSkill(playerSkills.skills[i].skill);
        }
    }

    void RefreshCooldowns()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (i >= playerSkills.skills.Length)
                break;

            SkillSlot slot = playerSkills.skills[i];

            if (slot.skill == null)
                continue;

            float remaining =
                Mathf.Max(
                    0,
                    slot.skill.cooldown -
                    (Time.time - slot.lastCastTime));

            float fill = remaining / slot.skill.cooldown;

            uiSlots[i].SetCooldown(fill);
        }
    }
}