using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateContent : MonoBehaviour
{

    /// <summary>
    /// True if ingredient is selected, menaing it is moved by the cursor gizmo. <br/>
    /// When dropped, this ingredient will be unselected and not removable (unless undone)
    /// </summary>
    public bool isSelected = false;

    /// <summary>
    /// Reference to the plate object, containing all ingredients
    /// </summary>
    public GameObject plateRef;

    void Start()
    {
        isSelected = true;

    }

    void Update()
    {
        if (isSelected && DragDetector.lastInstance != null) {
            transform.position = DragDetector.lastInstance.transform.position;
            if (DragDetector.lastInstance.touchCooldownCurrent < 0)
                drop();
        }
    }

    /// <summary>
    /// Called when this ingredient drops on the plate, unselecting and adding itself to the plate and cursors.
    /// </summary>
    void drop()
    {
        isSelected = false;
        // check if on plate, add to plate
    }
}
