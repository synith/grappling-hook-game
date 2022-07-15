using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private CollectableTypeSO collectableType;

    private void Awake()
    {
        collectableType = GetComponent<CollectableTypeHolder>().collectableType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            CollectCollectable();
    }

    private void CollectCollectable()
    {
        Destroy(gameObject);
        CollectableCounter.Instance.CollectableCollected(collectableType);
    }
}
