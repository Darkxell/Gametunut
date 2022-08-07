using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePassManager : MonoBehaviour
{

    /// <summary>
    /// No time to make a proper json for this, I'm sorry.
    /// </summary>
    public static BattlePassAward[] AwardsDatabase = new BattlePassAward[] { };

    public static BattlePassManager Instance;

    public GameObject Slider;

    public GameObject ContentView;

    void Awake()
    {
        Instance = this;
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
