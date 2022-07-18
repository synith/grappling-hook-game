using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCounter : MonoBehaviour
{
    public static CollectableCounter Instance { get; private set; }

    public event EventHandler OnCollecteableCollected;
    public event EventHandler OnAllCollectablesCollected;

    public int CollectablesCollectedAmount { get; private set; }
    public int CollectablesTotalAmount { get; private set; }

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
        CollectablesCollectedAmount++;

        OnCollecteableCollected?.Invoke(this, EventArgs.Empty);

        Debug.Log($"You've collected a {collectableType.collectableName}");

        if (CollectablesCollectedAmount >= CollectablesTotalAmount)
        {
            // Game Over
            OnAllCollectablesCollected?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetCollectablesTotalAmount(int collectablesTotalAmount)
    {
        CollectablesTotalAmount = collectablesTotalAmount;
    }
}
