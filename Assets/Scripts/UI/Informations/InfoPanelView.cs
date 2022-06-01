using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelView : MonoBehaviour
{

    public static InfoPanelView Instance;

    public GameObject TitleGameObject;

    void Awake()
    {
        Instance = this;
    }

}
