using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//GLOBAL EVENTS
[CreateAssetMenu(menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();
    public void TriggerEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            try
            {
                listeners[i].OnEventTriggered();
            }
            catch
            {
                RemoveListener(listeners[i]);
            }
        }
    }

    public void TriggerEvent(GameObject g)
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            try
            {
                listeners[i].OnEventTriggered(g);
            }
            catch { RemoveListener(listeners[i]); }
        }
    }
    public void AddListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}

//LOCAL EVENTS
[Serializable]
public class TransformListEvent : UnityEvent <List<Transform>> { };

[Serializable]
public class GameObjectEvent : UnityEvent <GameObject> { };
