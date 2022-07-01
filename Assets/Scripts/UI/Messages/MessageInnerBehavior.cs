using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Behavior of the inner view of a message, able to manipulate visual components to display custom data
/// </summary>
public class MessageInnerBehavior : MonoBehaviour
{

    public static MessageInnerBehavior Instance;

    public GameObject NameHeader;
    public GameObject ProfilePicture;
    public GameObject RecievedText;
    public GameObject ContentText;
    public GameObject NameSmall;
    public GameObject PictureSmall;

    public MessageInfo lastData = null;

    public void Awake()
    {
        Instance = this;
    }

    public void SetFromData(MessageInfo data)
    {
        lastData = data;
        // Text setters

        NameHeader.GetComponent<TextMeshProUGUI>().text = data.sender;
        RecievedText.GetComponent<TextMeshProUGUI>().text = "Release date : " + data.releasedate;
        ContentText.GetComponent<TextMeshProUGUI>().text = data.textlong; // TODO : textshort here is possible
        NameSmall.GetComponent<TextMeshProUGUI>().text = data.infos.nom;

        // Image setters

        ProfilePicture.GetComponent<Image>().sprite = Resources.Load<Sprite>("profiles/" + data.picture);
        PictureSmall.GetComponent<Image>().sprite = Resources.Load<Sprite>("profiles/" + data.picture);

    }
}
