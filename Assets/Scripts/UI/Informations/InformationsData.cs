using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Information table loaded from json file containing all info panel content links
/// </summary>
[Serializable]
public class InformationsData
{
    /// <summary>
    /// Name of the information categories available for help in the information pannel
    /// </summary>
    public string[] categories;

    /// <summary>
    /// Information categories content
    /// </summary>
    public InformationAsset[] content;

    private static InformationsData instance = null;

    /// <summary>
    /// Cached method to return all information data from the information json file. Heavy resource useage at first call, next to free after.
    /// </summary>
    /// <returns>An InformationData Object, containing information categories and </returns>
    public static InformationsData Get() {
        if (instance == null) {
            var jsonTextFile = Resources.Load<TextAsset>("Data/informations");
            instance = JsonUtility.FromJson<InformationsData>(jsonTextFile.text);
        }
        return instance;
    }

}

/// <summary>
/// Data about a single information pannel, used to create information lists and link inner content.
/// </summary>
[Serializable]
public class InformationAsset {

    /// <summary>
    /// name of the information asset, as displayed in the list
    /// </summary>
    public string name;

    /// <summary>
    /// name of the parent information category
    /// </summary>
    public string parent;

    /// <summary>
    /// path of the image used for this information pannel
    /// </summary>
    public string image;

}