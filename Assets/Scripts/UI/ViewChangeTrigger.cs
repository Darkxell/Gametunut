using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simplified monobehavior for viewchange trigger buttons, such as "close" or "back" ones.
/// </summary>
public class ViewChangeTrigger : MonoBehaviour
{

    public void changeView(int GameView)
    {
        if (ViewManager.Instance == null)
        {
            Debug.LogError("Fatal : ViewMnaager does not exist or has been culled, Game view may not be changed anymore.");
            return;
        }
        ViewManager.Instance.OnViewChange(GameView);
    }

}
