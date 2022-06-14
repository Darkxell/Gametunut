using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePassManager : MonoBehaviour
{

    public static BattlePassManager Instance;

    public GameObject Slider;

    void Awake()
    {
        Instance = this;
        close();
    }

    /// <summary>
    /// Opens the battlepass overlay UI above everything else (priority:100)
    /// </summary>
    public void open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Closes the battlepass overlay UI
    /// </summary>
    public void close()
    {
        gameObject.SetActive(false);
    }
}
