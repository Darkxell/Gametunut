using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoEnterButton : MonoBehaviour
{
    [HideInInspector]
    public string PanelName, PanelImagePath;

    public void OnClick()
    {
        Debug.Log("Clicked info button : " + PanelName + ", loading info panel at path " + PanelImagePath);

        ViewManager.Instance.OnViewChange(GameView.InfoSub2);

        // TODO: change infopanel content according to data above (title, back button on and image content)

    }




}
