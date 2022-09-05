using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingUIelement : MonoBehaviour
{

    public float rotatingspeed;

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotatingspeed, Space.Self);
    }
}
