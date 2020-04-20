using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnPrefab : MonoBehaviour
{
    [SerializeField] private GameObject trackpiece;
    [SerializeField] private float Yspacing;

    public GameObject getPrefab()
    {
        return trackpiece;
    }

    public float getSpacing()
    {
        return Yspacing;
    }
}
