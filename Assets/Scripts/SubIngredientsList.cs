using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubIngredientsList : MonoBehaviour, DragCallable
{

    /// <summary>
    /// List of sub ingredients prefabs in the scroll list. <br/>
    /// This list can be emptied and/or changed at any time by the main ingredientlist
    /// </summary>
    [HideInInspector]
    public List<GameObject> ingredients = new List<GameObject>(25);

    /// <summary>
    /// Reference to the header text to display current ingredients and debug info
    /// </summary>
    public GameObject HeaderTextRef = null;

    // Half horizontal size of the cull drag ui box, in unity world unit
    private float dragBoxSizeX = 0.4f;
    // Half vertical size of the cull drag ui box, in unity world unit
    private float dragBoxSizeY = 2f;

    private static float snapCooldown = 0.10f;
    private float snapCooldownCurrent = 0f;

    private float scroll = 0f;
    private readonly static float unitHeight = 0.8f;

    private int selectedID;

    void Update()
    {
        RebaseElements();
        snapCooldownCurrent -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        float autoscrollspeed = 0.05f;
        // If no scroll for a while, snap scroll to nearest element index.
        if (snapCooldownCurrent <= 0)
        {
            float scrolloffset = scroll % unitHeight;
            if (Mathf.Abs(scrolloffset) < autoscrollspeed * 2)
            {
                if (scrolloffset <= unitHeight / 2)
                    if (Mathf.Abs(scrolloffset) > 0.001f) scroll -= scrolloffset;
                    else if (Mathf.Abs(scrolloffset) < unitHeight - 0.001f) scroll += scrolloffset;
            }
            else
            {
                scroll += (scrolloffset >= unitHeight / 2) ? autoscrollspeed : -autoscrollspeed;
            }

        }
        // If outside array range, force go back in it.
        float maxscroll = (ingredients.Count - 1) * unitHeight;
        if (scroll < 0) scroll = 0;
        else if (scroll > maxscroll) scroll = maxscroll;
        // Update header text
        selectedID = (int)((scroll + unitHeight / 2f) / unitHeight);
        TextMeshPro tm = HeaderTextRef?.GetComponent<TextMeshPro>();
        if (tm != null)
        {
            tm.text = "Selected id : " + selectedID;
        }
    }

    /// <summary>
    /// Sets the positions of list elements according to the scroll value
    /// </summary>
    private void RebaseElements()
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            float yoffset = scroll - (i * unitHeight), yoffsetabs = Mathf.Abs(yoffset);
            float xoffset = (Mathf.Log10(yoffsetabs + 0.1f) + Mathf.Exp(Mathf.Pow(yoffsetabs, 2) / 10)) * 0.4f;
            ingredients[i].gameObject.transform.position = new Vector3(transform.position.x - xoffset, transform.position.y + yoffset, 0);
        }
    }

    public void OnDrag(Vector2 Position, Vector2 force, int actionID)
    {
        bool inside = Position.x < transform.position.x + dragBoxSizeX &&
            Position.x > transform.position.x - dragBoxSizeX &&
            Position.y < transform.position.y + dragBoxSizeY &&
            Position.y > transform.position.y - dragBoxSizeY;
        if (inside)
        {
            snapCooldownCurrent = snapCooldown;
            scroll += force.y;
        }
    }

    void OnDestroy()
    {
        // Unnessessary, but cleaner.
        DragDetector.lastInstance.callbacks.Remove(this);
    }
}
