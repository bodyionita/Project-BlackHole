using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationUI : MonoBehaviour
{
    public CardboardControl cardboard;

    // Event to announce exit button was pressed
    public delegate void ExitPressedHandler();
    public static event ExitPressedHandler OnExitPressed;

    private void OnEnable()
    {
        cardboard.trigger.OnDown += ClickPressed;
        cardboard.gaze.OnChange += ShowTooltips;
        DataProvider.OnSliceReady += UpdateDates;
    }

    private void OnDisable()
    {
        cardboard.trigger.OnDown -= ClickPressed;
        DataProvider.OnSliceReady -= UpdateDates;
    }

    private void ClickPressed(object sender)
    {
        GameObject objectGazed = cardboard.gaze.Object();

        string name = cardboard.gaze.IsHeld() ? objectGazed.name : "nothing";

        Debug.Log("Clicked while gazing at: " + name);

        // Empty click - pause/resume simulation
        if (name.Contains("nothing"))
        {
            SimManager.ins.ToggleSimulation();
            UpdateSimulationStatus();
        }

        if (name.Contains("BlackHole"))
        {
            SimManager.ins.StopSimulation();
        }
    }

    private void UpdateSimulationStatus()
    {
        var statusText = GameObject.Find("Status").GetComponentsInChildren<Text>()[1];

        if (SimManager.ins.pause)
        {
            statusText.text = "Paused";
            statusText.color = Color.yellow;
        }
        else
        {
            statusText.text = "Running";
            statusText.color = Color.green;
        }
    }

    private void UpdateDates(object sender)
    {
        Slider dateTracker = transform.GetComponentInChildren<Slider>();

        Text[] dateTexts = dateTracker.gameObject.transform.GetComponentsInChildren<Text>();
        float sliderValue = (float)((SimManager.ins.currentDate - SimManager.ins.startDate).TotalDays - (SimManager.ins.endDate - SimManager.ins.startDate).TotalDays);
        dateTracker.value = sliderValue;

        dateTexts[0].text = SimManager.ins.startDate.ToShortDateString();
        dateTexts[1].text = SimManager.ins.endDate.ToShortDateString();
        dateTexts[2].text = SimManager.ins.currentDate.ToShortDateString();
    }

    private void ShowTooltips(object sender)
    {
        GameObject objectGazed = cardboard.gaze.Object();
        string name = cardboard.gaze.IsHeld() ? objectGazed.name : "nothing";
        Debug.Log("Gazed changed: " + name);

        var actionText = GameObject.Find("Tooltip").GetComponentsInChildren<Text>()[1];


        if (name.Contains("BlackHole"))
        {
            actionText.text = "Click the blackhole to stop the simulation";
            actionText.color = Color.red;
        }

        if (name.Contains("Planet"))
        {
            actionText.text = "Click the any planet to view details";
            actionText.color = Color.white;
        }

        if (name.Contains("nothing"))
        {
            actionText.text = "Hover over objects to get tips";
        }
    }

}
