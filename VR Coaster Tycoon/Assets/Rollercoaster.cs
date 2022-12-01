using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Rollercoaster : MonoBehaviour
{
    // public List<GameObject> TrackPieces;
    public string rideName = "Ride1";
    private string savedRidesLocation = Application.streamingAssetsPath + "/SavedRides"; //change this to ride X later
    public List<GameObject> trackPieceList = new List<GameObject>();
    public Dictionary<string, GameObject> TrackPieces = new Dictionary<string, GameObject>();
    public Transform spawn;
    public pathVerticies myPath;
    public LastPieceDetector LastPieceDetector;


    private GameEventListener spawnListener = new GameEventListener();
    public GameEvent spawnEvent;
    private GameObjectEvent onTrackPieceSpawn = new GameObjectEvent();


    private void OnEnable()
    {
        spawnEvent.AddListener(spawnListener);
    }

    private void OnDisable()
    {
        spawnEvent.RemoveListener(spawnListener);
    }
    // Start is called before the first frame update
    void Start()
    {
        myPath.pieceDetector = LastPieceDetector;
        myPath.addPathPoints(gameObject);

        spawnListener.gameEvent = spawnEvent;
        onTrackPieceSpawn.AddListener(addTrackPiece);
        spawnListener.onGameObjectEventTriggered = onTrackPieceSpawn;

        //build directory later either change this to a stored directory or read from folder location
        foreach (GameObject g in trackPieceList)
            TrackPieces.Add(g.name, g); //doesn't check for same name!! there should be none

        DirectoryInfo rideLocation = new DirectoryInfo(savedRidesLocation);
        if (!rideLocation.Exists)
            rideLocation.Create();

        if(rideLocation.GetFiles().Length > 0)
        {
            FileInfo savedFile = rideLocation.GetFiles()[0];  //for now just get the first option
            processFile(savedFile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //read file and recreate previous build
    private void processFile(FileInfo savedFile)
    {
        int count = 0;
        foreach(string line in File.ReadAllLines(savedFile.FullName))
        {
            string[] split = line.Split(',');

            GameObject g = TrackPieces[split[0]]; //find trackpiece by name
            if (g != null)
            {
                //Vector3 spawnPoint = new Vector3(float.Parse(split[1]), float.Parse(split[1]), float.Parse(split[1]));
                g = Instantiate(g, spawn.position, Quaternion.LookRotation(spawn.forward, spawn.up) * g.transform.rotation);

                //leave last open so they can still build (Later change this to check if completed.  IF completed delete all)
                if (count < File.ReadAllLines(savedFile.FullName).Length)
                {
                    //foreach (Transform child in spawn.parent)
                    //{
                    //    Destroy(child.gameObject);
                    //}
                    Destroy(spawn.parent.gameObject);
                }

                myPath.addPathPoints(g);
                spawn = g.transform.GetChild(0).GetChild(0).GetChild(1); //gets the next spawn transform;

                count++;
            }

        }
    }

    //adds a track piece to the rides save file
    //Doesn't account for deleting/swaping pieces...
    //In the future deleting piece will delete all pieces after in game and in the file so this will just add on to the end of that file
    public void addTrackPiece(GameObject g)
    {
        string fileName = savedRidesLocation + "/" + rideName + ".txt";

        if (!File.Exists(fileName))
            File.Create(fileName);

        StreamWriter file = new StreamWriter(fileName, append: true); //might make this a private variable so it doesn't have to create each add

        file.WriteLine(g.name);
        file.Flush();
        file.Close();
    }
}
