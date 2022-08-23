using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    CollectableTypeSO _collectableType;

	CollectableCounter _collectableCounter;

	bool _isCollected;

    void OnEnable()
    {
		_collectableCounter = FindObjectOfType<CollectableCounter>();
	}

    void OnTriggerEnter(Collider other)
	{
		if (!_isCollected && other.CompareTag("Player"))
			CollectCollectable();
	}

	void CollectCollectable()
	{
		_isCollected = true;
        Destroy(gameObject);

		Instantiate(GameAssets.Instance.pf_CollectableEffect, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(SoundManager.Sound.Collected);

		_collectableCounter.CountCollectable(_collectableType);
	}
}
