using UnityEngine;

public class LootEntry : MonoBehaviour
{
    public GameObject lootPrefab;

    [Range(1, 100)]
    public int weight = 1;
}
