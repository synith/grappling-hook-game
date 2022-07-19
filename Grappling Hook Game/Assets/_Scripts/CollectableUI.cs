using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableUI : MonoBehaviour
{
    private TextMeshProUGUI _collectablesAcquiredText;
    private void Awake()
    {
        _collectablesAcquiredText = transform.Find("collectablesAcquiredText").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        CollectableCounter.Instance.OnCollecteableCollected += CollectableCounter_OnCollecteableCollected;
        SetCollectableText();
    }
    private void SetCollectableText()
    {
        _collectablesAcquiredText.text = $"{CollectableCounter.Instance.CollectablesCollectedAmount:00}/{CollectableCounter.Instance.CollectablesTotalAmount:00}";
    }    
    private void CollectableCounter_OnCollecteableCollected(object sender, System.EventArgs e)
    {
        SetCollectableText();
    }
}