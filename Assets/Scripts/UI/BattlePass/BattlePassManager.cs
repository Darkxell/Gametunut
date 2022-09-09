using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePassManager : MonoBehaviour
{

    /// <summary>
    /// No time to make a proper json for this, I'm sorry.
    /// </summary>
    public static BattlePassAward[] AwardsDatabase = new BattlePassAward[] {
        new BattlePassAward(50, "Ananas"),
        new BattlePassAward(152, 50),
        new BattlePassAward(205, "Escalope de dinde"),
        new BattlePassAward(305, 150),
        new BattlePassAward(355, "Cassis"),
        new BattlePassAward(455, "Crevettes"),
        new BattlePassAward(510, "Sardine"),
        new BattlePassAward(610, "Betterave"),
        new BattlePassAward(665, 500),
        new BattlePassAward(765, "Yaourt aromatisé"),
        new BattlePassAward(810, "Brochette de boeuf"),
        new BattlePassAward(920, "Pois chiches"),
        new BattlePassAward(970, 150),
        new BattlePassAward(1070, 50),
        new BattlePassAward(1120, "Poivron Jaune"),
        new BattlePassAward(1225, "Thon"),
        new BattlePassAward(1275, "Raisin"),
        new BattlePassAward(1375, 500),
        new BattlePassAward(1480, "Omelette"),
        new BattlePassAward(1530, "Fraise"),
        new BattlePassAward(1630, 50),
        new BattlePassAward(1685, "Myrtilles"),
        new BattlePassAward(1785, "Avocat"),
        new BattlePassAward(1840, 150),
        new BattlePassAward(1940, "Noix"),
        new BattlePassAward(1990, 500),
        new BattlePassAward(2100, "Chou-fleur")};

    public static BattlePassManager Instance;

    public GameObject Slider;

    public GameObject SubscriberText;
    public GameObject PublicationsText;

    public GameObject currentPointsText;

    public GameObject ContentView;

    public List<GameObject> objectFrames;

    public Sprite FrameLocked;
    public Sprite FrameUnlocked;

    public HashSet<string> ingredientsBlacklist = new HashSet<string>(25);

    /// <summary>
    /// Current battlepass completion, in points.
    /// Gets set on app launch depending on savestate progression, and updates when you gain points.
    /// </summary>
    private int currentpoints = 0;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        computeCurentPoints();
        close();
    }

    /// <summary>
    /// Opens the battlepass overlay UI above everything else (priority:100)
    /// </summary>
    public void open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Closes the battlepass overlay UI
    /// </summary>
    public void close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Forces a recompute of current points from the save data.
    /// This will also recompute and update the following:<br>
    ///  - amount of visible subscribers the player has.<br>
    ///  - number of visible publications on the player profile (just the numbers, not the posts themselves)<br>
    ///  - all content in the battlepass window<br>
    ///  - The HashSet of blacklisted ingredients because of the battlepass, if player has awards
    ///  <returns>A string of the newest unlocked ingredient ID, if this detected unlocking of a new reward. Null otherwise.</returns>
    /// </summary>
    public string computeCurentPoints()
    {
        string toreturn = null;
        int currentpointsbackup = currentpoints;
        // Save data gather
        string savedmissionsstr = PlayerPrefs.GetString("missions", "");
        string savedplatesstr = PlayerPrefs.GetString("plates", "");
        int amount_missions = savedmissionsstr.Equals("") ? 0 : savedmissionsstr.Split("|").Length, amount_plates = savedplatesstr.Equals("") ? 0 : savedplatesstr.Split("|").Length;
        // Points math
        currentpoints = 40 * amount_missions + 10 * amount_plates;
        if (GlobalManager.Instance.CurentTestClass == GlobalManager.TestClass.Class_Awards || GlobalManager.Instance.CurentTestClass == GlobalManager.TestClass.Class_AwardsStorytelling)
            currentpoints *= 7;
        // Subscribers math
        int subscribers = 250 + 7 * GlobalManager.Instance.CurrentDay + 13 * amount_plates;
        foreach (BattlePassAward award in AwardsDatabase)
            if (award.points <= currentpoints && award.type == BattlePassAwardType.Subscribers)
                subscribers += award.count;
        SubscriberText.GetComponent<TextMeshProUGUI>().text = subscribers + "\nAbonnés";
        PublicationsText.GetComponent<TextMeshProUGUI>().text = amount_plates + "\nPublications";
        // Slider graphical update
        Slider.GetComponent<Slider>().value = Mathf.Clamp(currentpoints, 0f, Slider.GetComponent<Slider>().maxValue);
        currentPointsText.GetComponent<TextMeshProUGUI>().text = "Points Actuels : " + currentpoints;
        ingredientsBlacklist.Clear();
        for (int i = 0; i < AwardsDatabase.Length; ++i)
            if (AwardsDatabase[i].points <= currentpoints) {
                objectFrames[i].GetComponent<Image>().sprite = FrameUnlocked;
                if (AwardsDatabase[i].points > currentpointsbackup && AwardsDatabase[i].type == BattlePassAwardType.Ingredient)
                    toreturn = AwardsDatabase[i].id;
            } else {
                objectFrames[i].GetComponent<Image>().sprite = FrameLocked;
                if (GlobalManager.Instance.hasAwards() && AwardsDatabase[i].type == BattlePassAwardType.Ingredient)
                    ingredientsBlacklist.Add(AwardsDatabase[i].id);
            }
        return toreturn;
    }
}

public class BattlePassAward
{

    public BattlePassAward(int points, int count)
    {
        this.points = points;
        this.type = BattlePassAwardType.Subscribers;
        this.count = count;
        this.id = "subscribers";
    }

    public BattlePassAward(int points, string id)
    {
        this.points = points;
        this.type = BattlePassAwardType.Ingredient;
        this.count = 1;
        this.id = id;
    }

    /// <summary>
    /// Number of points required to unlock this award
    /// </summary>
    public int points;

    public BattlePassAwardType type;

    /// <summary>
    /// How many of the award you get
    /// </summary>
    public int count;

    /// <summary>
    /// ID of the award, used notably for ingredient awards
    /// </summary>
    public string id;

}

public enum BattlePassAwardType
{
    Subscribers, Ingredient
}
