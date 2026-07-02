using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [Header("Drop Chance")]
    [Range(0, 100)]
    public int dropChance = 50;

    [Header("Loot Table")]
    public List<LootEntry> lootTable = new();

    [Header("Total Weight on Enemy - DO NOT OVERRIDE!")]
    [SerializeField] private int totalWeight;


    public void TryDropLoot()
    {
        int dropRoll = UnityEngine.Random.Range(0, 100);

        if (dropRoll >= dropChance)
        {
            return;
        }

        LootEntry selectedLoot = GetRandomLoot();

        if (selectedLoot == null)
            return;

        Instantiate(selectedLoot.lootPrefab, transform.position, Quaternion.identity);
    }


    LootEntry GetRandomLoot()
    {
        if (lootTable.Count == 0 || totalWeight <= 0)
            return null;

        int roll = UnityEngine.Random.Range(0, totalWeight);

        int currentWeight = 0;

        foreach (LootEntry entry in lootTable)
        {
            currentWeight += entry.weight;

            if (roll < currentWeight)
                return entry;
        }

        return null;
    }

    private void OnValidate()
    {
        totalWeight = 0;

        foreach (LootEntry entry in lootTable)
        {
            if (entry != null)
            {
                totalWeight += entry.weight;
            }
        }
    }
}