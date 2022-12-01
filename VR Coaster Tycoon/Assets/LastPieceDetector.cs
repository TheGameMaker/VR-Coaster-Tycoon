using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LastPieceDetector : MonoBehaviour
{
    private GameEventListener spawnListener = new GameEventListener();
    public GameEvent spawnEvent;
    private UnityEvent onTrackPieceSpawn = new UnityEvent();

    private void Start()
    {
        spawnListener.gameEvent = spawnEvent;
        onTrackPieceSpawn.AddListener(CheckTrackEnd);
        spawnListener.onUnityEventTriggered = onTrackPieceSpawn;
    }


    public void CheckTrackEnd()
    {
        RaycastHit hit;

        if(Physics.SphereCast(transform.position, 1f, -transform.forward, out hit, 1f))
        {
            Debug.Log("Detected " + hit.collider.gameObject.name + " " + hit.collider.gameObject.transform.position + " " + Vector3.Distance(hit.collider.transform.position, transform.position));
            GameObject g = hit.collider.gameObject;

            GameObject lastPiece = null;

            if (Vector3.Distance(hit.collider.transform.position, transform.position) < 5.1f)
            {
                if (g.transform.parent.parent.GetComponent<TrackPoints>() != null)
                {
                    lastPiece = g.transform.parent.parent.gameObject;
                }
                else if (g.transform.parent.GetComponent<TrackPoints>() != null)
                {
                    lastPiece = g.transform.parent.gameObject;
                }
                else if (g.GetComponent<TrackPoints>() != null)
                {
                    lastPiece = g.transform.gameObject;
                }
            }

            if (lastPiece != null)
            {
                lastPiece.transform.rotation = Quaternion.LookRotation((transform.GetChild(0).position - lastPiece.transform.position), lastPiece.transform.up);

                Destroy(lastPiece.transform.GetChild(0).GetChild(0).gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void OnEnable()
    {
        spawnEvent.AddListener(spawnListener);
    }

    private void OnDisable()
    {
        spawnEvent.RemoveListener(spawnListener);
    }
}
