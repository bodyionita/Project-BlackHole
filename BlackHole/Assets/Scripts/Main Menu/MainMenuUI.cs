using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public CardboardControl cardboard;

    private void OnEnable()
    {
        cardboard.trigger.OnClick += StartButtonPressed;
    }

    private void OnDisable()
    {
        cardboard.trigger.OnClick -= StartButtonPressed;
    }

    private void StartButtonPressed(object sender)
    {
        // Call game scene
        SceneLoader.LoadScene(SceneName.Simulation);
    }
}

