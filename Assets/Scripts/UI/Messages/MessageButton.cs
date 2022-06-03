using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageButton : MonoBehaviour
{

    public GameObject Image;
    public GameObject TextHead;
    public GameObject TextContent;

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

}
