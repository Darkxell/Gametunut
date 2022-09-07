using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

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
        // Text setters
        PosterName.GetComponent<TextMeshProUGUI>().text = Data.posterName;
        int jrsdiff = GlobalManager.Instance.CurrentDay - Data.day;
        DateText.GetComponent<TextMeshProUGUI>().text = Data.day == -1 ? "" : (jrsdiff == 0 ? "Aujourd'hui" : ("Il y a " + jrsdiff + " jours"));
        DescriptionText.GetComponent<TextMeshProUGUI>().text = Data.description;
        LikeText.GetComponent<TextMeshProUGUI>().text = Data.likeText;
        CommentsText.GetComponent<TextMeshProUGUI>().text = Data.comments;
        // Image setters
        ProfilePicture.GetComponent<Image>().sprite = Resources.Load<Sprite>("profiles/" + Data.profilePath);
        ContentImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("postImages/" + Data.imagePath);
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

/// <summary>
/// Singleton class to hold data about Feed publications that appear on the feed page.
/// </summary>
public static class PublicationDatabase
{

    private static Publication[] database = null;

    public static Publication[] get()
    {
        if (database != null) return database;
        Debug.Log("Reading Json information for daily posts in feed...");

        var jsonTextFile = Resources.Load<TextAsset>("Data/publicposts");
        database = JsonHelper.FromJson<Publication>(jsonTextFile.text);
        return database;
    }

}