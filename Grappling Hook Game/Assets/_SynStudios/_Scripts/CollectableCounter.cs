using System;
using UnityEngine;

public class CollectableCounter : MonoBehaviour
{
    public static event Action<int, int> OnCounted;
    public static event Action OnAllCollected;

    private int currentCollectablesAmount;
    private int totalCollectablesAmount;

    private void OnEnable()
    {
        Collectable.OnCollected += (collectableType) => CountCollectable(collectableType);
        CollectableSpawnManager.OnAllCollectablesSpawned += (totalCollectables) => SetTotalCollectablesAmount(totalCollectables);
    }

    private void OnDisable()
    {
        Collectable.OnCollected -= (collectableType) => CountCollectable(collectableType);
        CollectableSpawnManager.OnAllCollectablesSpawned -= (totalCollectables) => SetTotalCollectablesAmount(totalCollectables);
    }

    private void SetTotalCollectablesAmount(int totalCollectables)
    {
        totalCollectablesAmount = totalCollectables;
        OnCounted?.Invoke(currentCollectablesAmount, totalCollectablesAmount);
    }

    public void CountCollectable(CollectableTypeSO collectableType)
    {
        currentCollectablesAmount++;

        OnCounted?.Invoke(currentCollectablesAmount, totalCollectablesAmount);
        Debug.Log($"You've collected a {collectableType.collectableName}");

        if (currentCollectablesAmount >= totalCollectablesAmount)
        {
            Debug.Log("Game Over");
            OnAllCollected?.Invoke();
        }
    }
}
