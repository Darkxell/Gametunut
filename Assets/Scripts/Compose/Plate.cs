using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Plate behavior, containing ingredients in a specific disposition
/// </summary>
public class Plate : MonoBehaviour
{

    /// <summary>
    /// Current quest message active, relevent if the current view is a composer.
    /// May be null if there's no current quest, in which case the goal bars will be hidden.
    /// </summary>
    public MessageInfo currentQuest = null;

    /// <summary>
    /// Half size of the plate, around the middle pivot point.
    /// </summary>
    public float sizeX, sizeY;

    /// <summary>
    /// Prefab for plate content
    /// </summary>
    public GameObject PlateContentPrefab = null;

    /// <summary>
    /// Last created (started) instance of a plate. May be null or point to a deleted GameObject
    /// </summary>
    public static GameObject lastInstance = null;

    /// <summary>
    /// Child content of this plate in gameobjects form
    /// </summary>
    private List<GameObject> content = new List<GameObject>(10);

    /// <summary>
    /// Serializable mirror of the content in this plate
    /// </summary>
    private PlateInfo contentinfo = new PlateInfo();

    public GameObject contentSlider1, contentSlider2, contentSlider3, contentSlider4;

    void Awake()
    {
        lastInstance = gameObject;
    }

    private void Update()
    {
        Debug.Log("Plate update state : " + currentQuest + "\n"
            + "Plate content : " + content.Count + " items\n"
            + contentinfo);
    }

    public void addContent(PlateContent content)
    {
        GameObject c = content.gameObject;
        c.transform.parent = this.transform;
        contentinfo.content.Add(new PlateItem(c.transform.position.x, c.transform.position.y, content.data.name));
    }
    public void addContent(PlateItem content)
    {
        contentinfo.content.Add(content);
        GameObject pcontent = Instantiate(PlateContentPrefab, transform);
        pcontent.transform.position = new Vector3(content.x, content.y, pcontent.transform.position.z);
        pcontent.GetComponent<PlateContent>().data = IngredientsManager.getDataFor(content.ingredientID);
    }

    void instanciateFromData(PlateInfo data)
    {
        for (int i = 0; i < data.content.Count; i++)
            addContent(data.content[i]);
        // TODO: if this method is needed, add the gameobjects here at the correct locations
    }

    /// <summary>
    /// removes the current quest from the interface, if it exists.
    /// </summary>
    public void startQuestless()
    {
        currentQuest = null;
        contentSlider1.SetActive(false);
        contentSlider2.SetActive(false);
        contentSlider3.SetActive(false);
        contentSlider4.SetActive(false);
    }

    /// <summary>
    /// Initialises the plate and interface with the last data seen in the message inner behavior ui.
    /// </summary>
    public void startQuest()
    {
        if (MessageInnerBehavior.Instance.lastData == null)
        {
            startQuestless(); return;
        }

        currentQuest = MessageInnerBehavior.Instance.lastData;
        contentSlider1.SetActive(true);
        contentSlider2.SetActive(true);
        contentSlider3.SetActive(true);
        contentSlider4.SetActive(true);
    }

}

/// <summary>
/// Serializable object containing information about a plate, 
/// </summary>
[Serializable]
public class PlateInfo
{
    public List<PlateItem> content = new List<PlateItem>();

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[PlateInfo (" + content.Count + "): ");
        for (int i = 0; i < content.Count; i++)
            sb.Append(content[i]);
        sb.Append("]");
        return base.ToString();
    }
}

/// <summary>
/// Serializable item on a plate. contains an ingredient and a position.
/// </summary>
[Serializable]
public class PlateItem
{
    public float x, y;
    public string ingredientID;

    public PlateItem(float x, float y, string ingredientID)
    {
        this.x = x;
        this.y = y;
        this.ingredientID = ingredientID;
    }
    public override string ToString()
    {
        return "[" + ingredientID + "{" + x + "/" + y + "}]";
    }
}
