using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Specific behavior of page elements to show and hide them depending on the categories. <br/>
/// See:<code>public void OnViewChange(GameView target)</code>
/// </summary>
public class PageElement : MonoBehaviour
{
    /// <summary>
    /// Singleton like list of all page elements for quick iterating
    /// </summary>
    public static List<PageElement> elements = new List<PageElement>(20);

    /// <summary>
    /// Type of this Element, will only be active when the type here matches 
    /// </summary>
    public int GameViewType;

    private void Awake()
    {
        elements.Add(this);
    }
}
