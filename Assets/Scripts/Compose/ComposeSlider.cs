using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComposeSlider : MonoBehaviour
{
    public static GameObject[] sliders = new GameObject[4];

    /// <summary>
    /// ID of the slider from 0 to 3, et in the canvas prefab
    /// </summary>
    public int ID = -1;

    void Start()
    {
        if (ID >= 0)
            sliders[ID] = gameObject;
        else
            Debug.LogError("Slider IDs do not seem to be set in canvas prefab, please assign values from 0 to 3.");
    }

    void Update()
    {

    }

    /// <summary>
    /// Slider value recomputation cooldown
    /// </summary>
    int computeCooldown = 10, currentcooldown = 0;

    private void FixedUpdate()
    {
        currentcooldown--;
        if (currentcooldown <= 0)
        {
            currentcooldown = computeCooldown;
            float slidervalue = 0f;
            Plate p = Plate.lastInstance?.GetComponent<Plate>();
            if (p == null || !p.isActiveAndEnabled)
                goto skipcomputations;
            switch (ID)
            {
                default:
                    Debug.LogError("Uncomputable slider value for ID : " + ID + " (ID is out of bound)");
                    break;
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        skipcomputations:
            GetComponent<Slider>().value = slidervalue;
        }

    }
}
