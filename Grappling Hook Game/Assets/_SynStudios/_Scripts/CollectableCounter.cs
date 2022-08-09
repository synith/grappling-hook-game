using System;
using UnityEngine;

public class CollectableCounter : MonoBehaviour
{
    GameOverUI gameOverUI;

    public static event Action<int, int> OnCounted;

    private int currentCollectablesAmount;
    private int totalCollectablesAmount;

    private void OnEnable()
    {
        gameOverUI = FindObjectOfType<GameOverUI>();
        CollectableSpawnManager.OnAllCollectablesSpawned += (totalCollectables) => SetTotalCollectablesAmount(totalCollectables);
    }

    private void OnDisable()
    {
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
            currentCollectablesAmount = 0;
            Debug.Log("Game Over");
            gameOverUI.FinishGame();
        }
    }
}
