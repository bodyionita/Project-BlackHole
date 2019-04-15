using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public CardboardControl cardboard;

    // Event to announce start button was pressed
    public delegate void StartPressedHandler(int lYear, int rYear);
    public static event StartPressedHandler OnStartPressed;

    private void OnEnable()
    {
        cardboard.trigger.OnDown += ClickPressed;
    }

    private void OnDisable()
    {
        cardboard.trigger.OnDown -= ClickPressed;
    }

    private void ClickPressed(object sender)
    {
        GameObject objectGazed = cardboard.gaze.Object();

        string name = cardboard.gaze.IsHeld() ? objectGazed.name : "nothing";

        Debug.Log("Clicked while gazing at: " + objectGazed);

        // Call game scene
        if (name.Contains("Start"))
        {
            StartPressed();
        }

        // Show the dropdown options
        if (name.Contains("Dropdown"))
        {
            InteractionsUI.DropdownPressed(objectGazed);
        }

        // Select dropdown option
        if (name.Contains("Item"))
        {
            InteractionsUI.DropdownChoose(objectGazed);
        }
    }

    private void StartPressed()
    {
        var dropdownCanvases = transform.GetComponentsInChildren<Canvas>();
        Dropdown lDropdown, rDropdown;

        if (dropdownCanvases[1].name.Contains("Left"))
        {
            lDropdown = dropdownCanvases[1].GetComponentInChildren<Dropdown>();
            rDropdown = dropdownCanvases[2].GetComponentInChildren<Dropdown>();
        }
        else
        {
            lDropdown = dropdownCanvases[2].GetComponentInChildren<Dropdown>();
            rDropdown = dropdownCanvases[1].GetComponentInChildren<Dropdown>();
        }

        var lYear = int.Parse(lDropdown.options[lDropdown.value].text);
        var rYear = int.Parse(rDropdown.options[rDropdown.value].text);

        if (rYear <= lYear) rYear = lYear + 1;

        if (OnStartPressed != null) OnStartPressed(lYear, rYear);
    }

    
}

