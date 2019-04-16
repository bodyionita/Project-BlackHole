using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [SerializeField]
    private Planet planet = new Planet();
    [SerializeField]
    private int index = -1;

    // Motion controllers
    private ActiveController status = new ActiveController();
    private ActiveController previous_status = new ActiveController();

    // Components needed
    [SerializeField]
    private Transform orbitCentre;
    [SerializeField]
    private Transform planetCentre;
    [SerializeField]
    private Material planetMaterial;

    // Event to announce components have been gathered
    public delegate int ComponentsGatheredHandler(PlanetController pc);
    public static event ComponentsGatheredHandler OnComponentsGathered;

    // Event to announce the planet data and visualisation was updated
    public delegate void PlanetUpdatedHandler();
    public static event PlanetUpdatedHandler OnPlanetUpdated;

    public void UpdatePlanet(Planet p)
    {
        // Deactivate planet in order to make changes
        Hold();

        // Update planets parametres and visualisation
        planet = p;
        UpdateVisualisation();

        // Return planets status to the previous state
        Resume();
    }

    private void UpdateVisualisation()
    {
        // Update size
        planetCentre.localScale = new Vector3(planet.size, planet.size, planet.size);

        // Update distance 
        float currentDistance = Vector3.Distance(planetCentre.position, new Vector3(0, 0, 0));
        float ratio = planet.orbitRadius / currentDistance;
        planetCentre.position = new Vector3(planetCentre.position.x * ratio, planetCentre.position.y * ratio, planetCentre.position.z * ratio);

        // Update angle
        orbitCentre.Rotate(planet.orbitAngle - orbitCentre.rotation.x, 0, 0);

        // Update color
        planetMaterial.color = planet.color;

        // Update trail
        var trail = planetCentre.GetComponent<TrailRenderer>();
        trail.widthMultiplier = planet.size * 10 / planet.orbitPeriod;
        trail.time = planet.size / planet.orbitSpeed * 4;
        trail.minVertexDistance = planet.size * 0.2f;
        trail.material = planetMaterial;

        if (OnPlanetUpdated != null) OnPlanetUpdated();
    }    
    
    private void Hold()
    {
        if (status.isActive) previous_status.Activate();
        status.Deactivate();
    }

    private void Resume()
    {
        if (previous_status.isActive) status.Activate();
    }

    private void ChangeState(bool new_status, int i=-1)
    {
        if (index == i || i == -1)
        {
            if (new_status) status.Activate();
            else status.Deactivate();
        }
    }

    private void GatherComponents()
    {
        orbitCentre = GetComponent<Transform>();
        planetCentre = orbitCentre.GetChild(0).GetComponent<Transform>();
        planetMaterial = GetComponentInChildren<Renderer>().material;
        if (OnComponentsGathered != null)  index = OnComponentsGathered(this);
    }

    private void OnEnable()
    {
        PlanetsManager.OnStatusCommand += ChangeState;
        GatherComponents();
    }

    private void OnDisable()
    {
        PlanetsManager.OnStatusCommand -= ChangeState;
    }

    private void Update()
    {
        if (status.isActive)
        {
            orbitCentre.Rotate(0, planet.orbitSpeed * Time.deltaTime, 0);
        }
    }


}
