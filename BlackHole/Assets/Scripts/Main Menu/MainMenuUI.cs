using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void StartButtonPressed()
    {
        // Call game scene
        SceneLoader.LoadScene(SceneName.Simulation);
    }
}

