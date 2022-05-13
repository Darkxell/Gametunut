using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsList : MonoBehaviour, DragCallable
{
    /// <summary>
    /// List of ingredients prefabs in the scroll list
    /// </summary>
    [HideInInspector]
    public List<GameObject> ingredients = new List<GameObject>(25);

    private float scroll = 0f;
    private readonly static float unitHeight = 0.8f;

    // Half horizontal size of the cull drag ui box, in unity world unit
    public float dragBoxSizeX = 0.5f;
    // Half vertical size of the cull drag ui box, in unity world unit
    public float dragBoxSizeY = 2f;

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
            ingredients[i].gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - (i * unitHeight) + 1.3f + scroll, 0);
        }
    }

    public void OnDrag(Vector2 Position, Vector2 force, int actionID)
    {
        bool inside = Position.x < transform.position.x + dragBoxSizeY &&
            Position.x > transform.position.x - dragBoxSizeY &&
            Position.y < transform.position.y + dragBoxSizeX &&
            Position.y > transform.position.y - dragBoxSizeX;
        Debug.Log("list debug position : " + Position + " / force : " + force + " / inside : " + inside);
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
