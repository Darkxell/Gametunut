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

    private static float touchCooldown = 0.1f;
    public float touchCooldownCurrent = touchCooldown;

    private void Awake()
    {
        lastInstance = this;
    }

    /// <summary>
    /// Touch mode for the drag detector gizmo.<br/>
    /// Multitouch calls events for each touch independently with the position being the position of the touch and the force being calculated with the last update.<br/>
    /// Predictive calls events for single touch with position being the start of the touch, and 
    /// </summary>
    enum TouchMode
    {
        Multitouch, Predictive
    }

    private TouchMode currentTouchMode = TouchMode.Multitouch;

    void Update()
    {
        switch (currentTouchMode)
        {
            case TouchMode.Multitouch:
                for (int i = 0; i < Input.touchCount && i < positionBuffer.Length; i++)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                    Vector2 culledPos = new Vector2(touchPosition.x, touchPosition.y);
                    touchPosition.z = 0f;
                    if (i == 0)
                    {
                        transform.position = touchPosition;
                    }
                    if (Input.touches[i].phase == TouchPhase.Moved)
                        for (int j = 0; j < callbacks.Count; j++)
                        {
                            callbacks[j]?.OnDrag(culledPos, culledPos - positionBuffer[i], i);
                        }
                    positionBuffer[i] = culledPos;
                }
                if (Input.touchCount > 0)
                    touchCooldownCurrent = touchCooldown;
                else
                    touchCooldownCurrent -= Time.deltaTime;
                break;
            case TouchMode.Predictive:
                if (Input.touchCount <= 0)
                    break;
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                    Vector2 culledPos = new Vector2(touchPosition.x, touchPosition.y);
                    touchPosition.z = 0f;
                    transform.position = touchPosition;
                    if (Input.touches[0].phase == TouchPhase.Began)
                        positionBuffer[1] = touchPosition;
                    if (Input.touches[0].phase == TouchPhase.Moved)
                        for (int j = 0; j < callbacks.Count; j++)
                        {
                            callbacks[j]?.OnDrag(positionBuffer[1], culledPos - positionBuffer[0], 0);
                        }
                    positionBuffer[0] = culledPos;
                }
                break;
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
