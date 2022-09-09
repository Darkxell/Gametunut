using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StriesScrollLogger : MonoBehaviour
{

    bool s_2k, s_5k, s_10k, s_20k;

    void FixedUpdate()
    {
        float posy = Mathf.Abs(transform.position.y);
        if (!s_2k && posy >= 2000) {
            s_2k = true;
            GlobalManager.Instance.sendLogToServer("storyscroll,2000");
        }
        else if (!s_5k && posy >= 5000)
        {
            s_5k = true;
            GlobalManager.Instance.sendLogToServer("storyscroll,5000");
        }
        else if (!s_10k && posy >= 10000)
        {
            s_10k = true;
            GlobalManager.Instance.sendLogToServer("storyscroll,10000");
        }
        else if (!s_20k && posy >= 20000)
        {
            s_20k = true;
            GlobalManager.Instance.sendLogToServer("storyscroll,20000");
        }
    }
}
