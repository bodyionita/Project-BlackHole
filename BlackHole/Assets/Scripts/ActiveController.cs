using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveController
{
    private bool isActive;
    public bool IsActive
    {
        get { return isActive; }
    }

    public ActiveController()
    {
        isActive = false;
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    public bool Active()
    {
        return isActive;
    }

}
