using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageButton : MonoBehaviour
{

    public GameObject Image;
    public GameObject NotifIcon;
    public GameObject TextHead;
    public GameObject TextContent;

    public Sprite icon_new, icon_seen, icon_completed;

    public MessageInfo data;

    public void ChangeText(string content)
    {
        TextContent.GetComponent<TextMeshProUGUI>().text = content;
    }

    public void ChangeTextHeader(string content)
    {
        TextHead.GetComponent<TextMeshProUGUI>().text = content;
    }

    /// <summary>
    /// Changes the profile picture of the message sender in this button
    /// </summary>
    /// <param name="path">name of the profile picture file, without extension</param>
    public void ChangeImage(string path)
    {
        Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("profiles/" + path);
    }

    /// <summary>
    /// Changes the notification icon on the right to the desired sprite type
    /// </summary>
    public void ChangeNotifIcon(NotifIconType type)
    {
        switch (type)
        {
            case NotifIconType.New:
                NotifIcon.GetComponent<Image>().sprite = icon_new;
                break;
            case NotifIconType.Seen:
                NotifIcon.GetComponent<Image>().sprite = icon_seen;
                break;
            case NotifIconType.Completed:
                NotifIcon.GetComponent<Image>().sprite = icon_completed;
                break;
        }
    }

    /// <summary>
    /// Event called when 
    /// </summary>
    public void OnClick()
    {
        ViewManager.Instance.OnViewChange(GameView.MessageInner);
        MessageInnerBehavior.Instance.SetFromData(data);
    }

}

public enum NotifIconType
{
    New, Seen, Completed
}
