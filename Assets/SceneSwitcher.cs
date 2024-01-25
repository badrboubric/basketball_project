using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public GameObject GameView;
    public GameObject SettingsView;

    public void ToggleGameView()
    {
        // Check if the objects are not null to avoid errors
        if (SettingsView != null)
        {
            SettingsView.SetActive(false); // Turns off the first object
        }

        if (GameView != null)
        {
            GameView.SetActive(true); // Turns on the second object
        }
    }

    public void ToggleSettingsView()
    {
        // Check if the objects are not null to avoid errors
        if (SettingsView != null)
        {
            GameView.SetActive(false); // Turns off the first object
        }

        if (GameView != null)
        {
            SettingsView.SetActive(true); // Turns on the second object
        }
    }
}
