using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public SkillSlot[] skills;

    void Start()
    {
        foreach (SkillSlot slot in skills)
        {
            if (slot.skill != null)
            {
                slot.lastCastTime = -slot.skill.cooldown;
            }
        }
    }

    void Update()
    {
        foreach (SkillSlot slot in skills)
        {
            if (slot.skill == null)
                continue;

            if (!Input.GetKeyDown(slot.key))
                continue;

            if (!slot.Isready)
                continue;

            slot.lastCastTime = Time.time;

            slot.skill.behaviour.Cast(gameObject, slot.skill);
        }
    }
}