using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public enum SlotType
{
    Inventory,
    LeftHand,
    RightHand,
    Body,
    Head,
    Trash
}

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image icon;
    public SlotType slotType = SlotType.Inventory;

    [Header("Item Type")]
    public ItemType[] allowedType;
    [Header("Current Item")]
    public ItemData currentItem;

    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = icon.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = icon.gameObject.AddComponent<CanvasGroup>();
    }

    public void SetItem(ItemData item)
    {
        currentItem = item;

        if (item == null)
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
            icon.preserveAspect = false;
            return;
        }

        icon.sprite = item.icon;
        icon.color = Color.white;
        icon.preserveAspect = true;
        icon.raycastTarget = true;
    }

    public void ClearItem()
    {
        SetItem(null);
        icon.raycastTarget = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (icon.sprite == null) return;

        originalParent = icon.transform.parent;

        icon.transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        icon.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.transform.SetParent(originalParent);
        icon.transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot =
            eventData.pointerDrag.GetComponentInParent<InventorySlot>();

        if (draggedSlot == null || draggedSlot == this)
            return;

        if (draggedSlot.currentItem == null)
            return;

        if (slotType != SlotType.Inventory)
        {
            bool allowed = false;

            foreach (ItemType type in allowedType)
            {
                if (draggedSlot.currentItem.itemType == type)
                {
                    allowed = true;
                    break;
                }
            }

            if (!allowed)
                return;
        }

        //Trash Slot
        if (slotType == SlotType.Trash)
        {
            if (draggedSlot == null)
                return;

            draggedSlot.ClearSlot();

            EquipmentManager.Instance.UpdateStats();

            return;
        }

        ItemData tempItem = currentItem;

        SetItem(draggedSlot.currentItem);
        draggedSlot.SetItem(tempItem);

        EquipmentManager.Instance.UpdateStats();
    }

    public void ClearSlot()
    {
        currentItem = null;

        icon.sprite = null;
        icon.color = Color.clear;

        icon.preserveAspect = false;
    }
}