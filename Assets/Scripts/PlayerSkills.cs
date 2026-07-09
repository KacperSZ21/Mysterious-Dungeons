using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public SkillSlot[] skills;

    void Update()
    {
        foreach (SkillSlot slot in skills)
        {
            if (slot.skill == null)
                continue;

            if (!Input.GetKeyDown(slot.key))
                continue;

            if (Time.time < slot.lastCastTime + slot.skill.cooldown)
                continue;

            slot.lastCastTime = Time.time;

            slot.skill.behaviour.Cast(gameObject, slot.skill);
        }
    }
}