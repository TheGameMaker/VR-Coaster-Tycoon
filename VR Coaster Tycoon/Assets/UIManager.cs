using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIManager : MonoBehaviour
{

    public ToggleGroup BuildPieces, TrackSpeed;

    private GameEventListener spawnListener = new GameEventListener();
    public GameEvent SpawnRequest;
    private GameObjectEvent onSpawnRequest = new GameObjectEvent();

    public void assignToggleGroup(GameObject g)
    {
        Debug.Log("Event Triggered");
        if (g.GetComponent<SpawnPrefab>() != null)
        {
            g.GetComponent<SpawnPrefab>().trackGroup = BuildPieces;
        }
    }

    private void Start()
    {
        spawnListener.gameEvent = SpawnRequest;
        onSpawnRequest.AddListener(this.assignToggleGroup);
        spawnListener.onGameObjectEventTriggered = onSpawnRequest;
    }

    private void OnEnable()
    {
        SpawnRequest.AddListener(spawnListener);
    }

    private void OnDisable()
    {
        SpawnRequest.RemoveListener(spawnListener);
    }
}
