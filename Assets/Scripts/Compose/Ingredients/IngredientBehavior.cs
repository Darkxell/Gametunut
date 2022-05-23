using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Text;

/// <summary>
/// 
/// </summary>
public class IngredientBehavior : MonoBehaviour
{
    /// <summary>
    /// Data about this ingredient.<br/>
    /// Note that this instance may NOT be shared with the global dictionnary if it were to change during runtime, 
    /// but will share pointers for data linked after dictionnary deserialisation.
    /// </summary>
    public IngredientData data;

    public override string ToString()
    {
        return "Ingredient : " + data.name + " (" + data.energie + " Kcal)";
    }

}

/// <summary>
/// Serializable data about an ingredient.<br/>
/// https://docs.unity3d.com/Manual/JSONSerialization.html
/// </summary>
[Serializable]
public class IngredientData
{

    /// <summary>
    /// Name of the ingredient, used as its unique identifier
    /// </summary>
    public string name;
    /// <summary>
    /// sprite file name for menu and in scene
    /// </summary>
    public string spriteMenu, spriteInGame;

    /// <summary>
    /// In Kcal
    /// </summary>
    public float energie;

    /// <summary>
    /// In grams for 100 grams of product
    /// </summary>
    public float proteines, lipides, glucides, fibres;

    /// <summary>
    /// in miligrams for 100 grams of product
    /// </summary>
    public float fer, calcium, vitamineC;

    /// <summary>
    /// In micrograms for 100 grams of product
    /// </summary>
    public float vitamineA, vitamineB9, vitamineB12;

}

/// <summary>
/// Singleton class to generate a class based enum like sutructure of ingredients
/// </summary>
public static class IngredientsManager
{

    public static Boolean isSetup = false;

    private static Dictionary<String, IngredientData> content = new Dictionary<string, IngredientData>(1);
    private static List<String> Ingredients = new List<String>(1);

    public static void Setup()
    {
        isSetup = false;
        content = new Dictionary<string, IngredientData>(100);
        Ingredients = new List<String>(100);
        var jsonTextFile = Resources.Load<TextAsset>("Data/ingredients"); // no .json extension btw
        if (!jsonTextFile)
        {
            Debug.Log("Unable to setup info from Json file.");
            return;
        }
        Debug.Log("Reading Json information from file : " + jsonTextFile.name);
        IngredientData[] data = JsonHelper.FromJson<IngredientData>(jsonTextFile.text);
        Debug.Log("Found Json information for " + data.Length + " ingredients (global cached library)!");
        for (int i = 0; i < data.Length; i++)
        {
            content.TryAdd(data[i].name, data[i]);
            Ingredients.Add(data[i].name);
        }
        isSetup = true;
    }

    /// <summary>
    /// Try to return data for an ingredient ID (name). Will use a buffered cache of ingredient data similar ot class based enums.
    /// First call to this method is resource heavy, as it will load the full ingredient map. Loading can be preprocessed using <code>IngredientsManager.Setup();</code>.
    /// </summary>
    /// <param name="ingredientName">String name of the ingredient, as its unique identifier</param>
    /// <returns>Information about the parsed ingredient. Null if no ingredient of that name has been found.</returns>
    public static IngredientData getDataFor(String ingredientName)
    {
        if (!isSetup)
            Setup();
        IngredientData toreturn = null;
        content.TryGetValue(ingredientName, out toreturn);
        return toreturn;
    }

    public static List<String> getKeySet()
    {
        if (!isSetup)
            Setup();
        return Ingredients;
    }

}