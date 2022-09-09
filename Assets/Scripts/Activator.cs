using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior that activates all param scripts on awake. This is so they register to singletons and can deactivate themselves back, but be easily referenced later.
/// </summary>
public class Activator : MonoBehaviour
{

    /// <summary>
    /// List of subscribers that will be force activated once this object awakens (chain trigerring their own awake method).
    /// </summary>
    public List<GameObject> subscribers;

    public void Awake()
    {
        subscribers.ForEach(sub => sub.SetActive(true));
    }

}
