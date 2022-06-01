using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoEnterButton : MonoBehaviour
{
    [HideInInspector]
    public string PanelName, PanelImagePath;

    public void OnClick()
    {
        Debug.Log("Clicked info button : " + PanelName + ", loading info panel at path " + PanelImagePath);
        // Change infopanel title
        InfoPanelView.Instance.TitleGameObject.GetComponent<TextMeshProUGUI>().text = PanelName;
        // Change infopanel image content
        InfoPanelView.Instance.GetComponent<Image>().sprite = Resources.Load<Sprite>("info/" + PanelImagePath);
        // TODO: show/hide back button here

        ViewManager.Instance.OnViewChange(GameView.InfoSub2);
    }




}
