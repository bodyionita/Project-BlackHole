using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public Planet planet = new Planet(new Color(50, 50, 255));
    public ActiveController status = new ActiveController();

    public Transform orbitCentre;
    public Transform planetCentre;
    public Material planetMaterial;
    
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

        //Update speed
        planet.OnPeriodChanged();
    }

    public void UpdatePlanet(Planet p)
    {   
        planet = p;
        UpdateVisualisation();
    }


    private void OnEnable()
    {
        orbitCentre = GetComponent<Transform>();
        planetCentre = orbitCentre.GetChild(0).GetComponent<Transform>();
        planetMaterial = GetComponentInChildren<Renderer>().material;

    }

    private void Start()
    {
        if (orbitCentre == null || planetCentre == null || planetMaterial == null)
        {
            status.Deactivate();
            return;
        }
    }
    
    private void Update()
    {
        if (status.IsActive)
        {
            orbitCentre.Rotate(0, planet.OrbitSpeed * Time.deltaTime, 0);
        }
    }


}
