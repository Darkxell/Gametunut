using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Text;

/// <summary>
/// 
/// </summary>
public class CategoryBehavior : MonoBehaviour
{
    /// <summary>
    /// Data about this Category.<br/>
    /// Note that this instance may NOT be shared with the global dictionnary if it were to change during runtime, 
    /// but will share pointers for data linked after dictionnary deserialisation.
    /// </summary>
    public CategoryData data;

    private void Start()
    {

    }


}

/// <summary>
/// Serializable data about an ingredient category.<br/>
/// https://docs.unity3d.com/Manual/JSONSerialization.html
/// </summary>
[Serializable]
public class CategoryData
{

    /// <summary>
    /// Name of the ingredient, used as its unique identifier
    /// </summary>
    public string name;
    /// <summary>
    /// sprite file name for menu
    /// </summary>
    public string sprite;

    /// <summary>
    /// Array of contained ingredients in this category.
    /// </summary>
    public String[] ingredients;

}

/// <summary>
/// Singleton class to generate a class based enum like sutructure of ingredient categories 
/// </summary>
public static class CategoryManager
{

    public static Boolean isSetup = false;

    private static Dictionary<String, CategoryData> content = new Dictionary<string, CategoryData>(1);
    private static List<String> Categories = new List<String>(1);

    public static void Setup()
    {
        isSetup = false;
        content = new Dictionary<string, CategoryData>(100);
        Categories = new List<String>(100);
        var jsonTextFile = Resources.Load<TextAsset>("Data/categories"); // no .json extension btw
        if (!jsonTextFile) {
            Debug.Log("Unable to setup info from Json file.");
            return;
        }
        using (var streamReader = new StreamReader(jsonTextFile.text, Encoding.UTF8))
        {
            String textjson = streamReader.ReadToEnd();
            CategoryData[] data = JsonHelper.FromJson<CategoryData>(textjson);
            for (int i = 0; i < data.Length; i++)
            {
                content.TryAdd(data[i].name, data[i]);
                Categories.Add(data[i].name);
            }
        }
        isSetup = true;
    }

    /// <summary>
    /// Try to return data for an category ID (name). Will use a buffered cache of categories similar ot class based enums.
    /// First call to this method is resource heavy, as it will load the full category map. Loading can be preprocessed using <code>CategoryManager.Setup();</code>.
    /// </summary>
    /// <param name="categoryName">String name of the category, as its unique identifier</param>
    /// <returns>Information about the parsed category, containing its list of ingredients. Null if no ingredient of that name has been found.</returns>
    public static CategoryData getDataFor(String categoryName)
    {
        if (!isSetup)
            Setup();
        CategoryData toreturn = null;
        content.TryGetValue(categoryName, out toreturn);
        return toreturn;
    }

    public static List<String> getKeySet() {
        if (!isSetup)
            Setup();
        return Categories;
    }

}