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
    /// Reference to the back to quest button for show/hide purposes when you start a plate compose.
    /// </summary>
    public GameObject buttonBacktoquest;

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


    /// <summary>
    /// Sliders content : Energy, Proteins, Lipids, Glucids
    /// </summary>
    public GameObject contentSlider1, contentSlider2, contentSlider3, contentSlider4;

    /// <summary>
    /// Easily accessible mirrors for the slider values
    /// </summary>
    private float slider_energy_max = 2000, slider_proteins_max = 30, slider_lipids_max = 50, slider_glucids_max = 50,
        slider_energy_overcap = 2500, slider_proteins_overcap = 45, slider_lipids_overcap = 70, slider_glucids_overcap = 70,
        slider_energy_current = 0, slider_proteins_current = 0, slider_lipids_current = 0, slider_glucids_current = 0;

    void Awake()
    {
        lastInstance = gameObject;
    }

    private void Update()
    {
    }

    public void addContent(PlateContent content)
    {
        Debug.Log("Adding content to plate using PlateContent gameobject : " + content.transform);
        GameObject c = content.gameObject;
        c.transform.parent = this.transform;
        contentinfo.content.Add(new PlateItem(c.transform.position.x, c.transform.position.y, content.data.name));
        this.content.Add(content.gameObject);
        if (currentQuest != null)
            UpdateGauges();
    }
    public void addContent(PlateItem content)
    {
        Debug.Log("Adding content to plate using PlateItem data content : " + content);
        contentinfo.content.Add(content);
        GameObject pcontent = Instantiate(PlateContentPrefab, transform);
        pcontent.transform.position = new Vector3(content.x, content.y, pcontent.transform.position.z);
        pcontent.GetComponent<PlateContent>().data = IngredientsManager.getDataFor(content.ingredientID);
        if (currentQuest != null)
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
        if (currentQuest != null)
            UpdateGauges();
    }

    /// <summary>
    /// Removes all content added to this plate
    /// </summary>
    public void emptyPlate()
    {
        if (content.Count <= 0)
            return;


        content.ForEach(e => GameObject.Destroy(e));
        content.Clear();
        contentinfo.content.Clear();

        if (currentQuest != null)
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
        Debug.Log("Updating compose jauges (plate contains " + contentinfo.content.Count + " elements). Total energy : " + totalEnergy);
        // Computes the sliders max values
        slider_energy_max = 2000; slider_proteins_max = 30; slider_lipids_max = 50; slider_glucids_max = 50;
        slider_energy_overcap = 2500; slider_proteins_overcap = 45; slider_lipids_overcap = 70; slider_glucids_overcap = 70;
        try
        {
            float dailymultiplier = 0.2f; // math is for one day, this is to bring it to one meal, more or less
            float energyovercapmulti = 1.5f;
            float fourchette1a = 20, fourchette1b = 30, fourchette2a = 35, fourchette2b = 52, fourchette3a = 40, fourchette3b = 60;
            // fourchette B for overcap maximum. Could probbaly be a percentage, but actually correct this way. Edit: actually changet to ALWAYS be equivalent to a 50% more change, for balance. Also, I don't have time to make procedural sliders.
            SenderInfo personinfos = currentQuest.infos;
            // Energy
            double MB = (personinfos.sexe.Equals("Homme") ? 1.083 : 0.963) * Mathf.Pow(personinfos.poids, (float)0.48) * Mathf.Pow(personinfos.taille, (float)0.5) * Mathf.Pow(personinfos.age, -0.13f) * dailymultiplier;
            slider_energy_max = (int)(MB * 1.63 * 239);
            slider_energy_overcap = slider_energy_max * energyovercapmulti;
            // Proteins
            slider_proteins_max = (int)(MB * 1000 * (fourchette1a / 100) / 17);
            slider_proteins_overcap = (int)(MB * 1000 * (fourchette1b / 100) / 17);
            // Lipids
            slider_lipids_max = (int)(MB * 1000 * (fourchette2a / 100) / 38);
            slider_lipids_overcap = (int)(MB * 1000 * (fourchette2b / 100) / 38);
            // Glucids
            slider_glucids_max = (int)(MB * 1000 * (fourchette3a / 100) / 17);
            slider_glucids_overcap = (int)(MB * 1000 * (fourchette3b / 100) / 17);
        }
        catch (Exception)
        {
            Debug.LogError("Couldn't compute max slider vanules for quest giver. Using factory defaults.");
        }
        // logarithmic cheat to make things MUCH easier.
        if (totalEnergy > slider_energy_max)
        {
            totalEnergy = slider_energy_max + Mathf.Pow(totalEnergy - slider_energy_max, 0.5f);
        }
        if (totalProteins > slider_proteins_max)
        {
            totalProteins = slider_proteins_max + Mathf.Pow(totalProteins - slider_proteins_max, 0.4f);
        }
        if (totalLipids > slider_lipids_max)
        {
            totalLipids = slider_lipids_max + Mathf.Pow(totalLipids - slider_lipids_max, 0.5f);
        }
        if (totalGlucids > slider_glucids_max)
        {
            totalGlucids = slider_glucids_max + Mathf.Pow(totalGlucids - slider_glucids_max, 0.5f);
        }
        // Set the slider's max value
        contentSlider1.GetComponent<Slider>().maxValue = slider_energy_max * 2;
        contentSlider2.GetComponent<Slider>().maxValue = slider_proteins_max * 2;
        contentSlider3.GetComponent<Slider>().maxValue = slider_lipids_max * 2;
        contentSlider4.GetComponent<Slider>().maxValue = slider_glucids_max * 2;
        // Updates slider values
        contentSlider1.GetComponent<Slider>().value = Mathf.Clamp(totalEnergy, 0, slider_energy_max * 2);
        contentSlider2.GetComponent<Slider>().value = Mathf.Clamp(totalProteins, 0, slider_proteins_max * 2);
        contentSlider3.GetComponent<Slider>().value = Mathf.Clamp(totalLipids, 0, slider_lipids_max * 2);
        contentSlider4.GetComponent<Slider>().value = Mathf.Clamp(totalGlucids, 0, slider_glucids_max * 2);
        slider_energy_current = totalEnergy;
        slider_proteins_current = totalProteins;
        slider_lipids_current = totalLipids;
        slider_glucids_current = totalGlucids;
        // Complete log values
        Debug.Log("Changed slider values! For the current plate and client, here is the data:" +
            "\n[GOALS] Energy : " + slider_energy_max + " | Proteins : " + slider_proteins_max + " | Lipids : " + slider_lipids_max + " | Glucids : " + slider_glucids_max +
            "\n[CURRENT]" + totalEnergy + " | " + totalProteins + " | " + totalLipids + " | " + totalGlucids
            );
    }

    /// <summary>
    /// Validates this plate as completed.<br>
    /// Goes back to personal feed, saves the plate as completion, and if it was a mission, saves and resolves the mission attempt.
    /// </summary>
    public void ValidatePlate()
    {
        string unlock = null;
        bool questcomplete = checkQuestCompletion();
        Debug.Log("Validating plate!\nQuest :  " + currentQuest + "\n"
             + "Plate content : " + content.Count + " items\n"
             + contentinfo + "\nQuestComplete : " + questcomplete);
        // Saves the plate done in phone and profile feed
        if (questcomplete)
        {
            string savedplatesstr = PlayerPrefs.GetString("plates", "");
            PlayerPrefs.SetString("plates",
                savedplatesstr.Equals("") ?
                JsonUtility.ToJson(contentinfo) :
                (savedplatesstr + "|" + JsonUtility.ToJson(contentinfo))
                );
            // Saves mission completion
            if (currentQuest != null)
            {
                string savedmissionsstr = PlayerPrefs.GetString("missions", "");
                PlayerPrefs.SetString("missions",
                savedmissionsstr.Equals("") ?
                "" + currentQuest.id :
                (savedmissionsstr + "|" + currentQuest.id)
                );
                GlobalManager.Instance.sendLogToServer("questcomplete," + currentQuest.id);
            }
            // Updates battlepass and mission list
            unlock = BattlePassManager.Instance.computeCurentPoints();
            ButtonContainer.Instance.updateNotifIcons();
        }
        // Setup plate validation UI
        PlateValidationUI.Instance.ChangeContent(questcomplete, unlock != null, unlock, currentQuest != null);
        PlateValidationUI.Instance.gameObject.SetActive(true);
        // sends a server packet with data about the completion
        GlobalManager.Instance.sendLogToServer("platesubmit," + JsonUtility.ToJson(contentinfo) + "," + (currentQuest == null ? "null" : currentQuest.id));
    }

    /// <summary>
    /// Predicate that returns true if the curent plate is a good post for the current quest.
    /// Will always return false if the plate is empty.
    /// Will always return true if there's no current quest (unless empty)
    /// </summary>
    private bool checkQuestCompletion()
    {
        if (contentinfo.content.Count <= 0)
            return false;
        if (currentQuest == null)
            return true;
        bool whitelistcheck = true, blacklistcheck = true;
        for (int i = 0; i < currentQuest.whitelist.Length; i++)
            if (!contentinfo.containsTagID(currentQuest.whitelist[i]))
            {
                Debug.Log("Quest validation failed by missing whitelist tag : " + currentQuest.whitelist[i]);
                whitelistcheck = false;
            }
        for (int i = 0; i < currentQuest.blacklist.Length; i++)
            if (contentinfo.containsTagID(currentQuest.blacklist[i]))
            {
                blacklistcheck = false;
                Debug.Log("Quest validation failed by present blacklist tag : " + currentQuest.blacklist[i]);
            }
        bool barschecked = false;
        if (!currentQuest.respectbars)
            barschecked = true;
        else
            if (slider_energy_current <= slider_energy_overcap && slider_energy_current >= slider_energy_max
            && slider_glucids_current <= slider_glucids_overcap && slider_glucids_current >= slider_glucids_max
            && slider_lipids_current <= slider_lipids_overcap && slider_lipids_current >= slider_lipids_max
            && slider_proteins_current <= slider_proteins_overcap && slider_proteins_current >= slider_proteins_max)
            barschecked = true;


        Debug.Log("Checking if plate follows current quest standards!\n"
             + "Whitelist respected : " + whitelistcheck + " | Blacklist respected : " + blacklistcheck + " | Sliders respected : " + barschecked
             + "\nSLiders overview :\nEnergy : " + slider_energy_current + " / min goal : " + slider_energy_max + " / max goal : " + slider_energy_overcap
             + "\nGlucids : " + slider_glucids_current + " / min goal: " + slider_glucids_max + " / max goal: " + slider_glucids_overcap
             + "\nLipids : " + slider_lipids_current + " / min goal: " + slider_lipids_max + " / max goal: " + slider_lipids_overcap
             + "\nProteins : " + slider_proteins_current + " / min goal: " + slider_proteins_max + " / max goal: " + slider_proteins_overcap
             );

        return whitelistcheck && blacklistcheck && barschecked;
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
        emptyPlate();
        buttonBacktoquest.SetActive(false);
        GlobalManager.Instance.sendLogToServer("platestart,null");
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
        buttonBacktoquest.SetActive(true);
        GlobalManager.Instance.sendLogToServer("platestart," + currentQuest.id);
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

    public bool containsTagID(string id)
    {
        for (int i = 0; i < content.Count; i++)
        {
            IngredientData data = IngredientsManager.getDataFor(content[i].ingredientID);
            if (data == null)
                continue;
            for (int j = 0; j < data.tags.Count; j++)
                if (data.tags[j].Equals(id))
                    return true;
        }
        return false;
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
