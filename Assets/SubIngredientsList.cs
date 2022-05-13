using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubIngredientsList : MonoBehaviour, DragCallable
{

    /// <summary>
    /// List of sub ingredients prefabs in the scroll list. <br/>
    /// This list can be emptied and/or changed at any time by the main ingredientlist
    /// </summary>
    [HideInInspector]
    public List<GameObject> ingredients = new List<GameObject>(25);

    // Half horizontal size of the cull drag ui box, in unity world unit
    private float dragBoxSizeX = 0.4f;
    // Half vertical size of the cull drag ui box, in unity world unit
    private float dragBoxSizeY = 2f;

    private float scroll = 0f;
    private readonly static float unitHeight = 0.8f;

    void Start()
    {
        // TODO: replace this with procedural ingredients
        foreach (Transform child in transform)
            ingredients.Add(child.gameObject);
        DragDetector.lastInstance.callbacks.Add(this);
    }

    void Update()
    {
        RebaseElements();
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
            scroll += force.y;
        }
    }

    void OnDestroy()
    {
        // Unnessessary, but cleaner.
        DragDetector.lastInstance.callbacks.Remove(this);
    }
}
