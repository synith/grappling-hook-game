using System;
using UnityEngine;

public class CollectableCounter : MonoBehaviour
{
    public static event Action<int, int> OnCounted;
    public static event Action OnGameOver;

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

    private void Start()
    {
        OnCounted?.Invoke(currentCollectablesAmount, totalCollectablesAmount);
    }


    private void SetTotalCollectablesAmount(int totalCollectables)
    {
        totalCollectablesAmount = totalCollectables;
    }

    public void CountCollectable(CollectableTypeSO collectableType)
    {
        currentCollectablesAmount++;

        OnCounted?.Invoke(currentCollectablesAmount, totalCollectablesAmount);
        Debug.Log($"You've collected a {collectableType.collectableName}");

        if (currentCollectablesAmount >= totalCollectablesAmount)
        {
            Debug.Log("Game Over");
            OnGameOver?.Invoke();
        }
    }
}
