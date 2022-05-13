using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsList : MonoBehaviour, DragCallable
{
    /// <summary>
    /// List of ingredients categories prefabs in the scroll list
    /// </summary>
    [HideInInspector]
    public List<GameObject> categories = new List<GameObject>(25);

    /// <summary>
    /// Sub ingredients list, aka the wheel containing the drag/droppable elements on the right
    /// </summary>
    public GameObject sublist;

    private float scroll = 0f;
    private readonly static float unitHeight = 0.8f;

    // Half horizontal size of the cull drag ui box, in unity world unit
    private float dragBoxSizeX = 0.4f;
    // Half vertical size of the cull drag ui box, in unity world unit
    private float dragBoxSizeY = 2f;

    void Start()
    {
        // TODO: replace this with procedural ingredients
        foreach (Transform child in transform)
            categories.Add(child.gameObject);
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
        for (int i = 0; i < categories.Count; i++)
        {
            categories[i].gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - (i * unitHeight) + scroll, 0);
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
