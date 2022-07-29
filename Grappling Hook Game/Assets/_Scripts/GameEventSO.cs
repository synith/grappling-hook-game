using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameEvent")]
public class GameEventSO : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void TriggerEvent()
    {
        foreach (var listener in listeners)
        {
            listener.OnEventTriggered();
            Debug.Log("Event attempting to trigger");
        }
    }

    public void AddListener(GameEventListener listener) => listeners.Add(listener);

    public void RemoveListener(GameEventListener listener) => listeners.Remove(listener);
}
