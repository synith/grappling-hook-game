using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEventSO gameEvent;
    [SerializeField] private UnityEvent onEventTriggered;

    private void OnEnable() => gameEvent.AddListener(this);

    private void OnDisable() => gameEvent.RemoveListener(this);

    public void OnEventTriggered() => onEventTriggered.Invoke();
}
