using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemType
{
    None,
    Sword,
    Helmet,
    Armor,
    Shield,
    Gold
}

public enum DamageType
{
    Physical,
    Magical,
    Fire
}

public enum ArmorType
{
    Physical,
    Magical
}

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public Image[] slots;
    public Sprite swordIcon;

    [Header("Gold")]
    public TMP_Text goldText;

    private int gold = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGoldUI();
    }

    // Items
    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i].GetComponent<InventorySlot>();

            if (slot.currentItem == null)
            {
                slot.SetItem(item);
                return true;
            }
        }

        return false;
    }

    // Gold
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
    }

    void UpdateGoldUI()
    {
        goldText.text = gold.ToString();
    }
}