using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public CardboardControl cardboard;

    private void OnEnable()
    {
        cardboard.trigger.OnClick += ClickPressed;
    }

    private void OnDisable()
    {
        cardboard.trigger.OnClick -= ClickPressed;
    }

    private void ClickPressed(object sender)
    {
        string name = cardboard.gaze.IsHeld() ? cardboard.gaze.Object().name : "nothing";
        Debug.Log(cardboard.gaze.IsHeld());
        Debug.Log(cardboard.gaze.Object());
        // Call game scene
        if (name.Contains("Start")) SceneLoader.LoadScene(SceneName.LoadingScreen);
    }
}

