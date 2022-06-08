using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior of the inner view of a message, able to manipulate visual components to display custom data
/// </summary>
public class MessageInnerBehavior : MonoBehaviour
{

    public GameObject NameHeader;
    public GameObject ProfilePicture;
    public GameObject RecievedText;
    public GameObject ContentText;
    public GameObject NameSmall;
    public GameObject PictureSmall;

    public void SetFromData(MessageInfo data)
    {
        
    }
}
