using System;
using UnityEngine;

public class CollectableCounter : MonoBehaviour
{
    public static CollectableCounter Instance { get; private set; }

    public event EventHandler OnCollecteableCollected;
    public event EventHandler OnGameOver;

    public int CollectablesCollectedAmount { get; private set; }
    public int CollectablesTotalAmount { get; set; }

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
            Debug.Log("Game Over");
            OnGameOver?.Invoke(this, EventArgs.Empty);
        }
    }
}
