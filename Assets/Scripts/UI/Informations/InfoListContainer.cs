using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoListContainer : MonoBehaviour
{

    public GameObject InfopanelButtonPrefab;

    public static InfoListContainer Instance;

    public void Awake()
    {
        Instance = this;
    }

}
