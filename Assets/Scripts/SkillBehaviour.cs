using UnityEngine;

public abstract class SkillBehaviour : ScriptableObject
{
    public abstract void Cast(GameObject caster, SkillData data);
}