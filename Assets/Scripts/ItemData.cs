using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "RPG/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;

    public ItemType itemType;

    public Sprite icon;
    public bool hideHair = false;

    [Header("Stats")]
    public int damageBonus;
    public int healthBonus;
    public int defenseBonus;
    public DamageType damageType;
    public ArmorType armorType = ArmorType.Physical;
}