using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Database singleton to access all info about messages.
/// </summary>
public static class MessageDatabase
{

    private static MessageInfo[] database = null;

    private static void Setup()
    {
        var jsonTextFile = Resources.Load<TextAsset>("Data/messages"); // no .json extension btw
        if (!jsonTextFile)
        {
            Debug.Log("Unable to setup message database from Json file.");
            return;
        }
        Debug.Log("Reading Json information from file : " + jsonTextFile.name + "\nSetting up messages database...");
        database = JsonHelper.FromJson<MessageInfo>(jsonTextFile.text);
    }

    public static MessageInfo getFromID(int id)
    {
        if (database == null)
            Setup();
        for (int i = 0; i < database.Length; i++)
            if (database[i].id == id) return database[i];
        Debug.LogError("Cannot find message id " + id + " in database, returned null!");
        return null;
    }

    public static MessageInfo[] getList()
    {
        if (database == null)
            Setup();
        return database;
    }

}

[Serializable]
public class MessageInfo
{

    public int id;
    public string sender;
    public string picture;
    public string textshort;
    public string textlong;
    public string releasedate;
    public SenderInfo infos;
    public bool respectbars;
    public string[] whitelist;
    public string[] blacklist;

    public override string ToString()
    {
        return "[Message:" + id + " / From:" + infos.nom + "]";
    }
}

[Serializable]
public class SenderInfo
{
    public string nom;
    public string sexe;
    public int age;
    public int taille;
    public int poids;
    public float imc;
    public string probleme;
    public string requete;
}
