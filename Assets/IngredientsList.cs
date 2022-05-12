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
    float dragBoxSizeX = 2f;
    // Half vertical size of the cull drag ui box, in unity world unit
    float dragBoxSizeY = 0.5f;

    void Start()
    {
        // TODO: replace this with procedural ingredients
        foreach (Transform child in transform)
            ingredients.Add(child.gameObject);
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
            ingredients[i].gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - (i * unitHeight) - 2, 0);
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
}
