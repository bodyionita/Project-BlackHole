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
    private float _orbitPeriod;
    public float orbitPeriod
    {
        get { return _orbitPeriod; }
        set { _orbitPeriod = value; OnPeriodChanged(); }
    }

    [Range(0f, 45f)]
    public float orbitAngle;
    public float orbitSpeed { get; private set; }


    public Planet(Color _color, float _size=1f, float _orbitRadius=5f, float _orbitPeriod=5f, float _orbitAngle=0f)
    {
        color = _color;
        size = _size;
        orbitRadius = _orbitRadius;
        orbitPeriod = _orbitPeriod;
        orbitAngle = _orbitAngle;
    }

    private void OnPeriodChanged()
    {
        orbitSpeed = 360f / orbitPeriod;
    }

}
