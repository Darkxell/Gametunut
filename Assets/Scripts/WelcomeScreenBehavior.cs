using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeScreenBehavior : MonoBehaviour
{

    public static WelcomeScreenBehavior Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Toggles on the visibility of the welcome screen
    /// </summary>
    public void show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    ///  Event called when the continue button on the welcome screen is pressed. There's not much going on here btw...
    /// </summary>
    public void onButtonPress()
    {
        gameObject.SetActive(false);
    }

}
