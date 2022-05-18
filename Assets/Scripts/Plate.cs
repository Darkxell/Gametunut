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
    /// Child content of this plate in gameobjects form
    /// </summary>
    public List<GameObject> content = new List<GameObject>(10);

    private void Update()
    {
        
    }

    void instanciateFromData(PlateInfo data) { 
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
