using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class backgroundChanger : MonoBehaviour
{
    public Camera cameraToChange;
    private readonly string darkRedHex = "#310000"; // Hex code for dark red
    private readonly string darkGreenHex = "#023307"; // Hex code for dark green

    void Update()
    {
        if (cameraToChange == null)
        {
            Debug.LogError("Camera not assigned!");
            return;
        }

        string path = "C:/python/ready.json";
        if (File.Exists(path))
        {
            string jsonContent = File.ReadAllText(path);
            bool isGreen = bool.Parse(jsonContent);

            Color color;
            if (isGreen)
            {
                ColorUtility.TryParseHtmlString(darkGreenHex, out color);
            }
            else
            {
                ColorUtility.TryParseHtmlString(darkRedHex, out color);
            }

            cameraToChange.backgroundColor = color;
        }
        else
        {
            Debug.LogError("JSON file not found!");
        }
    }
}
