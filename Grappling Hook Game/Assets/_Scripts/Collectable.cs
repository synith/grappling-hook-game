using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private CollectableTypeSO collectableType;
    private bool _isCollected;
    private void Awake()
    {
        collectableType = GetComponent<CollectableTypeHolder>().collectableType;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isCollected)
            CollectCollectable();
    }
    private void CollectCollectable()
    {
        _isCollected = true;
        Destroy(gameObject);
        CollectableCounter.Instance.CollectableCollected(collectableType);
    }
}
