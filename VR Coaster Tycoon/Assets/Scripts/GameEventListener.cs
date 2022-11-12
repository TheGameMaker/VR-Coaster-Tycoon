using UnityEngine;
using UnityEngine.Events;
public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public UnityEvent onUnityEventTriggered;
    public GameObjectEvent onGameObjectEventTriggered;

    public void OnEventTriggered()
    {
        onUnityEventTriggered?.Invoke();
    }

    public void OnEventTriggered(GameObject g)
    {
        onGameObjectEventTriggered?.Invoke(g);
    }
}
