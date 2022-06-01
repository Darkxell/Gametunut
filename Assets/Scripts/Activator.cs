using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior that activates all param scripts on awake. This is so they register to singletons and can deactivate themselves back, but be easily referenced later.
/// </summary>
public class Activator : MonoBehaviour
{

    public List<GameObject> subscribers;

    public void Awake()
    {
        subscribers.ForEach(n => n.SetActive(true));
    }

}
