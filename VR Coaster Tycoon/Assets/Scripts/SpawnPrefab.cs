using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpawnPrefab : MonoBehaviour
{

    private GameObject prefab;
    private float spacing;
    [SerializeField] private Transform cube;
    [SerializeField] private Transform spawn;
    [SerializeField] public ToggleGroup trackGroup;
    [SerializeField] private bool deleteAll;
    [SerializeField] private bool ignoreSpacing;

    public string rideName = "Ride1";

    public void Start()
    {
       // Invoke("requestTrackGroup", 1f);
        requestTrackGroup();
    }

    public void Update()
    {
        //do nothing
    }

    private void requestTrackGroup()
    {
        SpawnRequest.TriggerEvent(this.gameObject);
    }

    public GameEvent OnSpawn, SpawnRequest;
    public void Spawn()
    {
        //Instantiate(prefab);
        //Instantiate(prefab, transform.position, prefab.transform.rotation);
        if (!trackGroup)
            SpawnRequest.TriggerEvent(this.gameObject);

        if (trackGroup != null)
        {
            List<Toggle> activeToggles = new List<Toggle>(trackGroup.ActiveToggles());

            foreach (Toggle toggle in activeToggles)
            {
                prefab = toggle.GetComponent<returnPrefab>().getPrefab();
                spacing = toggle.GetComponent<returnPrefab>().getSpacing();
            }

            if (!ignoreSpacing)
            {
                spawn.position = new Vector3(spawn.position.x, spawn.position.y + spacing, spawn.position.z);
            }

            GameObject g = Instantiate(prefab, spawn.position, Quaternion.LookRotation(spawn.forward, spawn.up) * prefab.transform.rotation);
            g.name = prefab.name;

           
            if (deleteAll)
            {
                foreach (Transform child in cube)
                {
                    Destroy(child.gameObject);
                }
                Destroy(cube.gameObject);
            }

            OnSpawn.TriggerEvent(g);
           // OnSpawn.TriggerEvent();
        }
    }


}
