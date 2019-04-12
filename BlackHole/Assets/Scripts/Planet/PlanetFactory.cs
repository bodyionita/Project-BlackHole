using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFactory : MonoBehaviour
{
    public GameObject planetPrefab;

    private GameObject SpawnPlanet(Transform container)
    {
        GameObject instance = Instantiate(planetPrefab, container) as GameObject;
        instance.transform.position = new Vector3(0, 0, 0);
        Debug.Log("Factory has instantiated: " + instance + " positioned at: " + instance.transform.position);
        return instance;
    }

    private void OnEnable()
    {
        PlanetsManager.OnSpawnRequest += SpawnPlanet;
    }

    private void OnDisable()
    {
        PlanetsManager.OnSpawnRequest -= SpawnPlanet;
    }

}
