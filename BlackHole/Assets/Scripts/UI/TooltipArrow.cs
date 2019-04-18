using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipArrow : MonoBehaviour
{
    private float _rotationPeriod;
    public float rotationPeriod
    {
        get { return _rotationPeriod; }
        set { _rotationPeriod = Mathf.Clamp(value, 1, 3); rotationSpeed = 360f / _rotationPeriod; }
    }

    private float _hopPeriod;
    public float hopPeriod
    {
        get { return _hopPeriod; }
        set { _hopPeriod = Mathf.Clamp(value, 1, 3); hopSpeed = hopHeight / _hopPeriod; }
    }

    private float rotationSpeed;
    private float hopHeight;
    private float hopUpperLimit;
    private float hopLowerLimit;
    private float hopSpeed;
    private int hopUpwards = 1;
    private void Awake()
    {
        hopHeight = 10;
        rotationPeriod = 5f;
        hopPeriod = 1f;
        hopUpperLimit = transform.localPosition.y + hopHeight;
        hopLowerLimit = transform.localPosition.y ;
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        if (transform.localPosition.y > hopUpperLimit) hopUpwards = -1;
        if (transform.localPosition.y < hopLowerLimit) hopUpwards = 1;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + hopUpwards * hopSpeed * Time.deltaTime, transform.localPosition.z);
    }
}
