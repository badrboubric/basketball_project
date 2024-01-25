using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textSwitcer : MonoBehaviour
{
    public void ToggleActive()
    {
        bool currentState = gameObject.activeSelf;
        gameObject.SetActive(!currentState);
    }
}
