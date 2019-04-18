using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Planet 
{
    public const float sizeLow = 1f;
    public const float sizeHigh = 4f;

    public const float orbitRadiusLow = 10f;
    public const float orbitRadiusHigh = 60f;

    public const float orbitPeriodLow = 10f;
    public const float orbitPeriodHigh = 30f;

    public const float orbitAngleLow = 0f;
    public const float orbitAngleHigh = 10f;

    [Range(sizeLow, sizeHigh), SerializeField]
    private float _size;
    public float size
    {
        get { return _size; }
        set { _size = Mathf.Clamp(value, sizeLow, sizeHigh);  }
    }
    
    [Range(orbitRadiusLow, orbitRadiusHigh), SerializeField]
    private float _orbitRadius;
    public float orbitRadius
    {
        get { return _orbitRadius; }
        set { _orbitRadius = Mathf.Clamp(value, orbitRadiusLow, orbitRadiusHigh); }
    }

    [Range(orbitPeriodLow, orbitPeriodHigh), SerializeField]
    private float _orbitPeriod;
    public float orbitPeriod
    {
        get { return _orbitPeriod; }
        set { _orbitPeriod = Mathf.Clamp(value, orbitPeriodLow, orbitPeriodHigh); orbitSpeed = 360f / _orbitPeriod; }
    }

    [Range(orbitAngleLow, orbitAngleHigh), SerializeField]
    private float _orbitAngle;
    public float orbitAngle
    {
        get { return _orbitAngle; }
        set { _orbitAngle = Mathf.Clamp(value, orbitAngleLow, orbitAngleHigh); }
    }

    public float orbitSpeed { get; private set; }
    public Color color;


    public Planet(float _size=sizeLow, float _orbitRadius=orbitRadiusLow, float _orbitPeriod=orbitPeriodLow, float _orbitAngle=orbitAngleLow)
    {
        color = Color.blue;
        size = _size;
        orbitRadius = _orbitRadius;
        orbitPeriod = _orbitPeriod;
        orbitAngle = _orbitAngle;
    }
}
