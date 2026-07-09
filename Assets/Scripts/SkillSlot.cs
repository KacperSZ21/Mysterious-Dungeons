using UnityEngine;

[System.Serializable]
public class SkillSlot
{
    public KeyCode key;

    public SkillData skill;

    [HideInInspector]
    public float lastCastTime;
}