using UnityEngine;
using UnityEngine.UI;


public class LootItem : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (InventoryUI.Instance.AddItem(itemData))
            {
                Destroy(gameObject);
            }
        }
    }
}