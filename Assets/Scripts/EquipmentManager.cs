using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public InventorySlot leftHand;
    public InventorySlot rightHand;
    public InventorySlot body;
    public InventorySlot head;

    private ClickToMove2D player;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = FindFirstObjectByType<ClickToMove2D>();

        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    public void UpdateStats()
    {
        int totalDamage = 0;
        int totalArmor = 0;
        int totalHealth = 0;

        DamageType attackType = DamageType.Physical;

        ArmorType armorType = ArmorType.Physical;

        // VISUALS
        Sprite headSprite = null;
        Sprite bodySprite = null;
        Sprite leftWeaponSprite = null;
        Sprite rightWeaponSprite = null;
        Sprite shieldSprite = null;

        bool hideHair = false;


        HandleSlot(head);
        HandleSlot(body);
        HandleSlot(leftHand);
        HandleSlot(rightHand);


        // APPLY STATS
        player.bonusDamage = totalDamage;
        player.armor = totalArmor;
        player.bonusHealth = totalHealth;

        player.currentDamageType = attackType;
        player.armorType = armorType;

        player.UpdateStatsUI();


        // APPLY VISUALS
        if (player.visuals != null)
        {
            player.visuals.SetHead(headSprite, hideHair);
            player.visuals.SetBody(bodySprite);
            player.visuals.SetLeftHand(leftWeaponSprite);
            player.visuals.SetRightHand(rightWeaponSprite);
            player.visuals.SetShield(shieldSprite);
        }



        // =========================
        // HANDLE SLOT
        // =========================

        void HandleSlot(InventorySlot slot)
        {
            if (slot == null)
                return;

            if (slot.currentItem == null)
                return;

            ItemData item = slot.currentItem;

            switch (item.itemType)
            {
                // WEAPONS
                case ItemType.Sword:
                    totalDamage += item.damageBonus;

                    attackType = item.damageType;

                    if (slot == leftHand)
                    {
                        leftWeaponSprite = item.icon;
                    }
                    else if (slot == rightHand)
                    {
                        rightWeaponSprite = item.icon;
                    }

                    break;


                // SHIELD
                case ItemType.Shield:

                    totalArmor += item.defenseBonus;

                    shieldSprite = item.icon;

                    break;


                // HELMET
                case ItemType.Helmet:

                    totalHealth += item.healthBonus;

                    headSprite = item.icon;

                    hideHair = item.hideHair;

                    break;


                // BODY ARMOR
                case ItemType.Armor:

                    totalArmor += item.defenseBonus;

                    armorType = item.armorType;

                    bodySprite = item.icon;

                    break;
            }
        }
    }
}