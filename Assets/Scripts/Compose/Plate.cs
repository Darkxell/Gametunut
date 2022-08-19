using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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
    }

    public void addContent(PlateContent content)
    {
        GameObject c = content.gameObject;
        c.transform.parent = this.transform;
        contentinfo.content.Add(new PlateItem(c.transform.position.x, c.transform.position.y, content.data.name));
        this.content.Add(content.gameObject);
    }
    public void addContent(PlateItem content)
    {
        contentinfo.content.Add(content);
        GameObject pcontent = Instantiate(PlateContentPrefab, transform);
        pcontent.transform.position = new Vector3(content.x, content.y, pcontent.transform.position.z);
        pcontent.GetComponent<PlateContent>().data = IngredientsManager.getDataFor(content.ingredientID);
        UpdateGauges();
    }

    /// <summary>
    /// Removes the last content added to this plate
    /// </summary>
    public void removeContent()
    {
        if (content.Count <= 0 || content.Count != contentinfo.content.Count)
            return;
        int removeIndex = content.Count - 1;
        GameObject.Destroy(content[removeIndex]);
        content.RemoveAt(removeIndex);
        contentinfo.content.RemoveAt(removeIndex);
        UpdateGauges();
    }


    public void UpdateGauges()
    {
        // Computes the total amount of Nutritional values in the plate for the sliders
        float totalEnergy = 0f, totalProteins = 0f, totalLipids = 0f, totalGlucids = 0f;
        for (int i = 0; i < contentinfo.content.Count; i++)
        {
            IngredientData datai = IngredientsManager.getDataFor(contentinfo.content[i].ingredientID);
            if (datai != null)
            {
                totalEnergy += datai.energie;
                totalProteins += datai.proteines;
                totalLipids += datai.lipides;
                totalGlucids += datai.glucides;
            }
        }
        // Computes the sliders max values
        contentSlider1.GetComponent<Slider>().maxValue = 2100;
        contentSlider2.GetComponent<Slider>().maxValue = 300;
        contentSlider3.GetComponent<Slider>().maxValue = 1200;
        contentSlider4.GetComponent<Slider>().maxValue = 1600;
    }

    /// <summary>
    /// Validates this plate as completed.<br>
    /// Goes back to personal feed, saves the plate as completion, and if it was a mission, saves and resolves the mission attempt.
    /// </summary>
    public void ValidatePlate()
    {
        Debug.Log("Validating plate!\nQuest :  " + currentQuest + "\n"
             + "Plate content : " + content.Count + " items\n"
             + contentinfo);
        // Switches the UI panel to main
        ViewManager.Instance.OnViewChange(GameView.Profil);
        // Saves the plate done in phone and profile feed
        string savedplatesstr = PlayerPrefs.GetString("plates", "");
        PlayerPrefs.SetString("plates", (savedplatesstr.Equals("") ? "" : savedplatesstr + "|") + JsonUtility.ToJson(contentinfo));
        // Saves mission completion
        if (currentQuest != null /* && TODO : Check if mission is complete and won */)
        {
            string savedmissionsstr = PlayerPrefs.GetString("missions", "");
            PlayerPrefs.SetString("missions", (savedmissionsstr.Equals("") ? "" : savedmissionsstr + "|") + "|" + currentQuest.id);
        }
        BattlePassManager.Instance.computeCurentPoints();
        // sends a server packet with data about the completion
        // TODO

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
        return sb.ToString();
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
