using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsManager : MonoBehaviour
{
    public GameObject planetContainer;

    void Start()
    {
        
    }

    void OnEnable()
    {
        float x = 1000f;
        foreach (PlanetController planetController in planetContainer.transform.GetComponentsInChildren<PlanetController>())
        {
            if (!planetController.status.Active())
            {
                Planet p = new Planet(new Color(255- x / 6, x / 12, 0), x / 500, x / 5, x / 100, 0*(x / 60 - 15));
                planetController.UpdatePlanet(p);
                planetController.status.Activate();
            }
            x += 1;
        }
        
    }
}
