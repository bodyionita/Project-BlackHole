using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveController
{
    private bool _isActive;
    public bool isActive
    {
        get { return _isActive; }
    }

    public ActiveController()
    {
        _isActive = false;
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

}
