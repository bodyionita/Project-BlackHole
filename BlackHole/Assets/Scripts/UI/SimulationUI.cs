using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

using MongoDB.Bson;
public class SimulationUI : MonoBehaviour
{
    public CardboardControl cardboard;
    public Transform worldTooltip;
    public Transform overlayUI;
    public Transform worldUI;
    public Transform infoUI;
    public RawImage logoImage;

    // Event to announce exit button was pressed
    public delegate void ExitPressedHandler();
    public static event ExitPressedHandler OnExitPressed;

    private void OnEnable()
    {
        cardboard.trigger.OnClick += ClickPressed;
        cardboard.gaze.OnChange += ShowTooltips;
        DataProvider.OnSliceReady += UpdateDates;
        SimManager.OnSimulationStatusUpdated += UpdateSimulationStatus;
    }

    private void OnDisable()
    {
        cardboard.trigger.OnClick -= ClickPressed;
        DataProvider.OnSliceReady -= UpdateDates;
        SimManager.OnSimulationStatusUpdated += UpdateSimulationStatus;

    }

    private void ClickPressed(object sender)
    {
        GameObject objectGazed = cardboard.gaze.Object();
        string name = cardboard.gaze.IsHeld() ? objectGazed.name : "nothing";

        
        if (infoUI.gameObject.activeSelf)
        {
            infoUI.gameObject.SetActive(false);
        }
        else
        {
            // Empty click - pause/resume simulation
            if (name.Contains("nothing"))
            {
                SimManager.ins.SetSimulation(SimManager.ins.pause);
            } else

            // Blackhole click - exit simulation
            if (name.Contains("BlackHole"))
            {
                SimManager.ins.StopSimulation();
            } else

            // Plane click - show stock details
            if (name.Contains("Planet Model"))
            {
                SimManager.ins.SetSimulation(false);
                infoUI.gameObject.SetActive(true);
                var planetIndex = objectGazed.transform.parent.GetComponent<PlanetController>().index;
                Debug.Log("clicked on planet: " + planetIndex);

                ShowInfoOnPlanet(planetIndex);
            }

           
        }
    }

    private void ShowInfoOnPlanet(int planetIndex)
    {
        var parameters = GameObject.FindGameObjectsWithTag("params");
        var data = SimManager.ins.currentSlice[DataStorage.ins.symbolNames[planetIndex].ToString()];

        StartCoroutine(DownloadImage(data["logo_url"].ToString()));

        var floatParams = new List<string>() { "ttmEPS", "latestEPS", "open", "close", "high", "low", "pe", "vtp", "change" };

        foreach (var name in parameters)
        {
            var textField = name.GetComponent<Text>();
            var text = data[name.name].ToString();
            if (floatParams.Contains(name.name))
            {
                text = Math.Round(data[name.name].ToDouble(), 2).ToString();
            }
            if (name.name.Contains("change"))
            {
                text += "%";
            }
            textField.text = text;
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

        var actionText = GameObject.Find("Tooltip").GetComponentsInChildren<Text>()[1];

        if (name.Contains("BlackHole"))
        {
            ShowTooltipOnObject("Exit");

            actionText.text = "Click the blackhole to stop the simulation";
            actionText.color = Color.red;
        }

        if (name.Contains("Planet Model"))
        {
            var planetIndex = objectGazed.transform.parent.GetComponent<PlanetController>().index;
            ShowTooltipOnObject(DataStorage.ins.symbolNames[planetIndex].AsString);

            actionText.text = "Click to view details of planet";
            actionText.color = Color.white;
        }

        if (name.Contains("nothing"))
        {
            worldTooltip.gameObject.SetActive(false);
            actionText.text = "Click to pause/resume";
        }
    }


    private void ShowTooltipOnObject(string text)
    {
        worldTooltip.position = cardboard.gaze.Object().transform.position;

        var distance = Vector3.Distance(worldTooltip.position, Camera.main.transform.parent.position);

        worldTooltip.localScale = new Vector3(distance * 0.001f, distance * 0.001f, distance * 0.001f);
        worldTooltip.rotation = Camera.main.transform.parent.rotation;


        var angleToGoUp = Mathf.Rad2Deg * Mathf.Asin(cardboard.gaze.Object().transform.localScale.x / distance) * 5f / cardboard.gaze.Object().transform.localScale.x;

        worldTooltip.RotateAround(Camera.main.transform.parent.position, Vector3.left, angleToGoUp );

        worldTooltip.GetComponent<TMP_Text>().SetText(text);
        worldTooltip.gameObject.SetActive(true);
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            logoImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }

}
