using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Planet 
{
    [Range(1f, 50f)]
    public float size;

    public Color color;
    
    [Range(5f, 1000f)]
    public float orbitRadius;
    
    [Range(5f, 30f)]
    private float orbitPeriod;
    public float OrbitPeriod
    {
        get { return orbitPeriod; }
        set { orbitPeriod = value; OnPeriodChanged(); }
    }

    [Range(0f, 45f)]
    public float orbitAngle;
    
    private float orbitSpeed;
    public float OrbitSpeed
    {
        get { return orbitSpeed; }
        set { orbitSpeed = value; }
    }


    public Planet(Color _color, float _size=1f, float _orbitRadius=5f, float _orbitPeriod=5f, float _orbitAngle=0f)
    {
        color = _color;
        size = _size;
        orbitRadius = _orbitRadius;
        OrbitPeriod = _orbitPeriod;
        orbitAngle = _orbitAngle;
    }

    public void OnPeriodChanged()
    {
        OrbitSpeed = 360f / OrbitPeriod;
    }

}
