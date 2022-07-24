using UnityEngine;

public class Collectable : MonoBehaviour
{
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
        CollectableCounter.Instance.CollectableCollected(_collectableType);
		SoundManager.Instance.PlaySound(SoundManager.Sound.Collected);
    }
}
