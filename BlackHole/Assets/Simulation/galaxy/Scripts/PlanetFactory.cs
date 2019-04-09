using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFactory : MonoBehaviour
{
    [Range(0f, 2000f)]
    public float maxPlanets;

    public GameObject planetPrefab;
    public GameObject container;

    void Start()
    {
        container.SetActive(false);
        int maximum = (int)maxPlanets;
        for (int i=0; i<maximum; i++)
        {
            GameObject instance = Instantiate(planetPrefab, container.transform) as GameObject;
            instance.transform.position = new Vector3(0, 0, 0);
            Debug.Log("Factory has instantiated: " + instance + " positioned at: " + instance.transform.position);
        }
        Debug.Log("Factory has activated the container");
        container.SetActive(true);

    }
}
