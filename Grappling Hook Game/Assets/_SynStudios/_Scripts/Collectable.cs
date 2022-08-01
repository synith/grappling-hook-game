using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public static event Action<CollectableTypeSO> OnCollected;

	[SerializeField]private CollectableTypeSO _collectableType;
    private bool _isCollected;

	//Audio Source that plays pulsating hum noise on awake/loop

    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !_isCollected)
			CollectCollectable();
	}

	private void CollectCollectable()
	{
		_isCollected = true;
        Destroy(gameObject);
		OnCollected?.Invoke(_collectableType);
		SoundManager.Instance.PlaySound(SoundManager.Sound.Collected);
    }
}
