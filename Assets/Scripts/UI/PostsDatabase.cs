using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Database singleton to access all info about public posts in feed.
/// </summary>
public static class PostsDatabase
{

    private static PublicPostData[] database = null;

    private static void Setup()
    {
        var jsonTextFile = Resources.Load<TextAsset>("Data/publicposts"); // no .json extension btw
        if (!jsonTextFile)
        {
            Debug.Log("Unable to setup public posts database from Json file.");
            return;
        }
        Debug.Log("Reading Json information from file : " + jsonTextFile.name + "\nSetting up public posts feed database...");
        database = JsonHelper.FromJson<PublicPostData>(jsonTextFile.text);
    }

    public static PublicPostData[] getList()
    {
        if (database == null)
            Setup();
        return database;
    }

}

[Serializable]
public class PublicPostData
{

    public string posterName;
    public string profilePath;
    public int day;
    public string description;
    public bool isPlate;
    public string imagePath;
    public string likeText;
    public string comments;
}
