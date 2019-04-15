using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class InteractionsUI
{
    public static void DropdownPressed(GameObject obj)
    {
        var dropdown = obj.GetComponent<Dropdown>();
        if (dropdown) dropdown.Show();

        var optionsCanvas = obj.GetComponentInChildren<Canvas>();
        optionsCanvas.overrideSorting = false;
    }

    public static void DropdownChoose(GameObject obj)
    {
        var dropdown = obj.transform.parent.parent.parent.parent.GetComponent<Dropdown>();

        var optionText = obj.GetComponentInChildren<Text>();

        var options = new List<string>();
        foreach (var o in dropdown.options)
        {
            options.Add(o.text);
        }
        dropdown.value = options.FindIndex(x => x.Contains(optionText.text));
        dropdown.Hide();
    }
}
