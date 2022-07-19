using UnityEngine;

public class Collectable : MonoBehaviour
{
	[SerializeField]private CollectableTypeSO _collectableType;
	private bool _isCollected;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !_isCollected)
			CollectCollectable();
	}

	private void CollectCollectable()
	{
		_isCollected = true;
		Destroy(gameObject);
		CollectableCounter.Instance.CollectableCollected(_collectableType);
	}
}
