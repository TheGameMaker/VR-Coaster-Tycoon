using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPrefab : MonoBehaviour
{

    private GameObject prefab;
    private float spacing;
    [SerializeField] private Transform cube;
    [SerializeField] private Transform spawn;
    [SerializeField] private ToggleGroup trackGroup;
    [SerializeField] private bool deleteAll;
    [SerializeField] private bool ignoreSpacing;

    public void Start()
    {
        trackGroup = FindObjectOfType<ToggleGroup>();
    }

    public void Update()
    {
        //do nothing
    }

    public void Spawn()
    {
        //Instantiate(prefab);
        //Instantiate(prefab, transform.position, prefab.transform.rotation);
        List<Toggle> activeToggles = new List<Toggle>(trackGroup.ActiveToggles());

        foreach(Toggle toggle in activeToggles)
        {
            prefab = toggle.GetComponent<returnPrefab>().getPrefab();
            spacing = toggle.GetComponent<returnPrefab>().getSpacing();
        }

        if (!ignoreSpacing)
        {
            spawn.position = new Vector3(spawn.position.x, spawn.position.y + spacing, spawn.position.z);
        }
        Instantiate(prefab, spawn.position, prefab.transform.rotation);

        if (deleteAll)
        {
            foreach (Transform child in cube)
            {
                Destroy(child.gameObject);
            }
            Destroy(cube.gameObject);
        }

    }
}
