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
    /// Data about the ingredient. May be null, with a 0 info ingredient and a default texture
    /// </summary>
    public IngredientData data = null;

    /// <summary>
    /// True once this component has set its own price according to its data (if not null)
    /// </summary>
    private bool spriteSet = false;

    void Start()
    {
        isSelected = true;

    }

    void Update()
    {
        if (isSelected && DragDetector.lastInstance != null)
        {
            transform.position = DragDetector.lastInstance.transform.position;
            if (DragDetector.lastInstance.touchCooldownCurrent < 0)
                drop();
        }

        if (!spriteSet && data != null)
        {
            spriteSet = true;
            //TODO: set sprite here
        }
    }

    /// <summary>
    /// Called when this ingredient drops on the plate, unselecting and adding itself to the plate and cursors.
    /// </summary>
    void drop()
    {
        isSelected = false;
        if (Plate.lastInstance != null && Plate.lastInstance.activeSelf)
        {
            // check if on plate, add to plate

            Plate.lastInstance.GetComponent<Plate>().addContent(this);




        }
    }
}
