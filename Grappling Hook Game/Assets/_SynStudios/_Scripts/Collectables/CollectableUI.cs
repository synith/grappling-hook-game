using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableUI : MonoBehaviour
{
    private TextMeshProUGUI collectablesAcquiredText;

    private void Awake()
    {
        collectablesAcquiredText = transform.Find("collectablesAcquiredText").GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        CollectableCounter.OnCounted += (current, total) => SetCollectableText(current, total);
    }

    private void OnDisable()
    {
        CollectableCounter.OnCounted -= (current, total) => SetCollectableText(current, total);
    }
        
    private void SetCollectableText(int currentCollectablesAmount, int totalCollectablesAmount)
    {
        collectablesAcquiredText.SetText($"{currentCollectablesAmount:00}/{totalCollectablesAmount:00}");
    }
}