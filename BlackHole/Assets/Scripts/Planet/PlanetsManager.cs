using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsManager : MonoBehaviour
{
    public GameObject planetContainer;
    float x = 1000f;

    void Start()
    {
        
    }

    void OnEnable()
    {
        PlanetController.OnComponentsGathered += TestPlanets;
    }

    void TestPlanets(PlanetController pc)
    {
        Planet p = new Planet(new Color(255 - x / 6, x / 12, 0), x / 500, x / 5, x / 100, 0 * (x / 60 - 15));
        pc.UpdatePlanet(p);
        pc.status.Activate();
        x += 1;
    }

    private void OnDisable()
    {
        PlanetController.OnComponentsGathered -= TestPlanets;
    }

}
