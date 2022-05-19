using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plate behavior, containing
/// </summary>
public class Plate : MonoBehaviour
{

    /// <summary>
    /// Last created (started) instance of a plate. May be null or point to a deleted GameObject
    /// </summary>
    public static GameObject lastInstance = null;

    /// <summary>
    /// Child content of this plate in gameobjects form
    /// </summary>
    private List<GameObject> content = new List<GameObject>(10);

    /// <summary>
    /// Serializable mirror of the content in this plate
    /// </summary>
    private PlateInfo contentinfo = new PlateInfo();

    void Start()
    {
        lastInstance = gameObject;
    }


    public void addContent(PlateContent content)
    {
        // TODO
    }
    public void addContent(PlateItem cotnent)
    {
        // TODO
    }

    void instanciateFromData(PlateInfo data) 
    { 
        // TODO: actually make this happen
    }

}

/// <summary>
/// Serializable object containing
/// </summary>
[Serializable]
public class PlateInfo
{
    public List<PlateItem> content = new List<PlateItem>();
}

/// <summary>
/// Serializable item on a plate. contains an ingredient and a position.
/// </summary>
[Serializable]
public class PlateItem
{
    public float x, y;
    public string ingredientID;
}
