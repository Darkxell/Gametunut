using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AboutPanelSetter : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "Identifiant unique : " + GlobalManager.Instance.playerIDShort;
    }
}
