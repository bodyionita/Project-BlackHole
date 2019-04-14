using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public CardboardControl cardboard;

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
        if (name.Contains("Start")) SceneLoader.LoadScene(SceneName.LoadingScreen);

        // Show the dropdown options
        if (name.Contains("Dropdown"))
        {
            var dropdown = objectGazed.GetComponent<UnityEngine.UI.Dropdown>();
            Debug.Log("Found dropdown to show: " + dropdown);
            if (dropdown) dropdown.Show();

            var optionsCanvas = objectGazed.GetComponentInChildren<Canvas>();
            Debug.Log("Found options canvas to disable sorting: " + optionsCanvas);
            optionsCanvas.overrideSorting = false;

        }

        // Select dropdown option
        if (name.Contains("Item"))
        {
            var dropdown = objectGazed.transform.parent.parent.parent.parent.GetComponent< UnityEngine.UI.Dropdown >();
            Debug.Log("Found dropdown for changing: " + dropdown);

            var optionText = objectGazed.GetComponentInChildren<UnityEngine.UI.Text>();
            Debug.Log("Found option text of choosing: " + optionText);

            var options = new List<string>();
            foreach (var o in dropdown.options)
            {
                options.Add(o.text);
            }
            dropdown.value = options.FindIndex(x => x.Contains(optionText.text));
            dropdown.Hide();
        }

    }
}

