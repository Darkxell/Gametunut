using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PublicationBehavior : MonoBehaviour
{
    public GameObject ProfilePicture;
    public GameObject PosterName;
    public GameObject DateText;
    public GameObject DescriptionText;
    public GameObject ContentImage;
    public GameObject LikeImage;
    public GameObject LikeText;
    public GameObject CommentsText;

    /// <summary>
    /// Sets the GUI elements of this publication object to fit the wanted data
    /// </summary>
    public void SetFromData(Publication Data)
    {

    }
}

/// <summary>
/// Serializable publication class that contains the inner informations needed to create publication ui element from json dataset.
/// </summary>
[Serializable]
public class Publication
{

    public string posterName;
    public string profilePath;
    public int day;

    public string description;

    /// <summary>
    /// True if the publication image is a plate, false if it is an image
    /// </summary>
    public bool isPlate;

    public string imagePath;
    // public PlateInfo plate;

    public string likePath;

    public string likeText;
    public string comments;
}
