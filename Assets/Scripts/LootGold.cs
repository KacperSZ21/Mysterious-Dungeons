using UnityEngine;

public class LootGold : MonoBehaviour
{
    public int goldAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryUI.Instance.AddGold(goldAmount);
            Destroy(gameObject);
        }
    }
}