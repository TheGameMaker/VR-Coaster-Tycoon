using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class pathVerticies : MonoBehaviour
{
    public List<Vector3> pathPoints;
    Vector3 Origin, Rotation;
    //private void OnDrawGizmosSelected()
    //{
    //   // Debug.Log("Vertice Length");
    //    // midPoints = new List<Vector3>();
    //    Vector3[] sortedList = GetComponent<MeshFilter>().sharedMesh.vertices;//.OrderBy(v => v.x).ToArray<Vector3>();
    //    Origin = transform.position;
    //    Rotation = transform.rotation.eulerAngles;

    //    //    startVEC = sortedList[lowesti] - sortedList[0];
    //    //    //Debug.Log(lowestDot + " POINT " + lowesti);
    //    //}
    //   // Debug.Log($"rotation {Rotation.x} {Rotation.y} {Rotation.z}");// + sortedList.Length);

    //    //loop through list of vertices
    //    for (int i = 1; i < sortedList.Length; i++)
    //    {

    //        Gizmos.color = Color.red;
    //        Vector3 pos = sortedList[i];//new Vector3(sortedList[i].x + Origin.x, sortedList[i].y + Origin.y, sortedList[i].z + Origin.z);

    //        //X Rotation
    //        float theta = (float)Math.Round(Rotation.x, 2) * Mathf.Deg2Rad;
    //        pos.y = (pos.y * Mathf.Cos(theta)) - (pos.z * Mathf.Sin(theta));
    //        pos.z = (pos.y * Mathf.Sin(theta)) + (pos.z * Mathf.Cos(theta));

    //        //Y Rotation
    //        theta = (float)Math.Round(Rotation.y, 2) * Mathf.Deg2Rad;
    //        pos.x = (pos.x * Mathf.Cos(theta)) + (pos.z * Mathf.Sin(theta));
    //        pos.z = (pos.z * Mathf.Cos(theta)) - (pos.x * Mathf.Sin(theta));

    //        //Z Rotation
    //        theta = (float)Math.Round(Rotation.y, 2) * Mathf.Deg2Rad;
    //        pos.x = (pos.x * Mathf.Cos(theta)) - (pos.y * Mathf.Sin(theta));
    //        pos.y = (pos.x * Mathf.Sin(theta)) + (pos.y * Mathf.Cos(theta));

    //        //Translation
    //        pos = new Vector3(pos.x + Origin.x, pos.y + Origin.y, pos.z + Origin.z);

    //        Gizmos.DrawSphere(pos, 0.1f);



    //    }
    //}
    // Start is called before the first frame update

    //private Vector3 previousMidpoint, midPoint;
    //private List<Vector3> midPoints;


    private GameEventListener spawnListener = new GameEventListener();
    public GameEvent spawnEvent;
    private GameObjectEvent onTrackPieceSpawn = new GameObjectEvent();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 point in pathPoints)
            Gizmos.DrawSphere(point, 0.1f);
    }
    public void addPathPoints(List<Transform> newPoints)
    {
        foreach (Transform t in newPoints)
            pathPoints.Add(t.position);
    }

    public void addPathPoints(GameObject g)
    {
        if (g.GetComponent<TrackPoints>() != null)
        {
            foreach (Transform t in g.GetComponent<TrackPoints>().TrackPiecePoints)
                pathPoints.Add(t.position);
        }
    }

    private void Start()
    {
        spawnListener.gameEvent = spawnEvent;
        onTrackPieceSpawn.AddListener(this.addPathPoints);
        spawnListener.onGameObjectEventTriggered = onTrackPieceSpawn;
       // spawnListener.onGameObjectEventTriggered.AddListener(this.addPathPoints);

        if(FindObjectOfType<TrackPoints>() != null)
        {
            addPathPoints(FindObjectOfType<TrackPoints>().gameObject);
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
