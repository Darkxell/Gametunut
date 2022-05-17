using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singletion like behavior to detect up to 20 drag points on the screen
/// </summary>
public class DragDetector : MonoBehaviour
{

    public static DragDetector lastInstance = null;

    /// <summary>
    /// List of callback objects that will be notified on drag events
    /// </summary>
    [HideInInspector]
    public List<DragCallable> callbacks = new List<DragCallable>(10);

    private Vector2[] positionBuffer = new Vector2[10];

    private void Awake()
    {
        lastInstance = this;
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount && i < positionBuffer.Length; i++)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
            Vector2 culledPos = new Vector2(touchPosition.x, touchPosition.y);
            touchPosition.z = 0f;
            if (i == 0) {
                transform.position = touchPosition;
            }
            for (int j = 0; j < callbacks.Count; j++)
            {
                if (Input.touches[i].phase == TouchPhase.Moved)
                    callbacks[j]?.OnDrag(culledPos, culledPos - positionBuffer[i], i);
            }
            positionBuffer[i] = culledPos;
        }

    }

}

/// <summary>
/// Interface for world elements that have specific interactions with a drag action.
/// Will be called by this dragDetector
/// </summary>
public interface DragCallable
{

    /// <summary>
    /// Event called by a DragDetector each update per drag event. Positions are in world position.
    /// </summary>
    public abstract void OnDrag(Vector2 Position, Vector2 force, int actionID);

}
