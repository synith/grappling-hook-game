using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCounter : MonoBehaviour
{
    public static CollectableCounter Instance { get; private set; }


    private int collectablesCollectedAmount;
    private int collectablesTotalAmount;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CollectableCollected(CollectableTypeSO collectableType)
    {
        collectablesCollectedAmount++;
        Debug.Log($"You've collected a {collectableType.collectableName}");
        Debug.Log($"{collectablesCollectedAmount} / {collectablesTotalAmount} collectables collected");
        if (collectablesCollectedAmount >= collectablesTotalAmount)
        {
            // Game Over, You win.
            Debug.Log("Game Over!");
        }
    }

    public void SetCollectablesTotalAmount(int collectablesTotalAmount)
    {
        this.collectablesTotalAmount = collectablesTotalAmount;
    }
}
