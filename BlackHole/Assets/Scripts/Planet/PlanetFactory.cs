using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFactory : MonoBehaviour
{
    public static PlanetFactory ins;

    public GameObject planetPrefab;

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
    }

    public void SpawnPlanet(Transform container)
    {
        GameObject instance = Instantiate(planetPrefab, container) as GameObject;
        instance.transform.position = new Vector3(0, 0, 0);
    }
}
