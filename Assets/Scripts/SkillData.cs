using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill")]
public class SkillData : ScriptableObject
{
    [Header("General")]
    public string skillName;

    // Skill icon for UI
    public Sprite icon;

    public float cooldown = 5f;

    [Header("Damage")]
    public int damage;
    public DamageType damageType = DamageType.Physical;

    [Header("Area")]
    public float radius = 3f;

    [Header("Buff")]
    public int bonusDamage;
    public float buffDuration;

    [Header("Heal")]
    public int healAmount;

    [Header("Visuals")]
    public GameObject particlePrefab;

    [Header("Behaviour")]
    public SkillBehaviour behaviour;
}