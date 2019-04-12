using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsManager : MonoBehaviour
{
    private int totalPlanets = 0;
    private int _loadedPlanets = 0;
    private int loadedPlanets
    {
        get { return _loadedPlanets; }
        set { _loadedPlanets = value; if (_loadedPlanets == totalPlanets) OnPlanetsLoaded(); }
    }

    // Planet container
    [SerializeField]
    private Transform container;

    // Event to check all planets have been fully loaded
    public delegate void PlanetsLoadedHandler();
    public static event PlanetsLoadedHandler OnPlanetsLoaded;

    // Event to wait for component gathering
    public delegate void StatusCommandHandler(bool new_status, int planetIndex=-1);
    public static event StatusCommandHandler OnStatusCommand;

    // Event to handle spawn request
    public delegate GameObject SpawnRequestHandler(Transform c);
    public static event SpawnRequestHandler OnSpawnRequest;

    // Keep track of the planets controller by their assigned indexes
    private Dictionary<int, PlanetController> IndexToControllerMap = new Dictionary<int, PlanetController>();    

    public void UpdatePlanets(Dictionary<int, Planet> planets)
    {
        foreach (var planet in planets)
        {
            PlanetController pc = IndexToControllerMap[planet.Key];
            pc.UpdatePlanet(planet.Value);
        }
    }

    public void SpawnRequest(int requested_number)
    {
        totalPlanets += requested_number;
        for (int i = 1; i <= requested_number; i++)
            if (OnSpawnRequest != null) OnSpawnRequest(container);
    }


    public void SetPlanets(bool active)
    {
        if (OnStatusCommand != null) OnStatusCommand(active);
    }    

    private void OnEnable()
    {
        PlanetController.OnComponentsGathered += PlanetLoaded;
    }

    private void OnDisable()
    {
        PlanetController.OnComponentsGathered -= PlanetLoaded;
    }

    private int PlanetLoaded(PlanetController pc)
    {
        loadedPlanets += 1;
        IndexToControllerMap[loadedPlanets] = pc;
        return loadedPlanets;
    }

}
