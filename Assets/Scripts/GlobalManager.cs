using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Networking;

public class GlobalManager : MonoBehaviour
{

    public static GlobalManager Instance;

    /// <summary>
    /// The current day in the experiment. Starts at day 0, and increases every real life day by one.<br>
    /// Only updates on app launch, opening the app at 23:59 lets you play the previous day still.
    /// </summary>
    public int CurrentDay;

    // mm/dd/yyyy US format : https://docs.microsoft.com/fr-fr/dotnet/api/system.datetime.parse?view=net-6.0
    public DateTime StartDate = System.DateTime.Parse("06/08/2022");

    /// <summary>
    /// Hashed player ID, unique per device
    /// </summary>
    [HideInInspector]
    public string playerID, playerIDShort;

    /// <summary>
    /// The test class of the current game instance, dicated by the device ID
    /// </summary>
    public TestClass CurentTestClass = TestClass.Undefined;

    void Awake()
    {
        Instance = this;
        // Computes the current experiment day
        CurrentDay = Mathf.Max(0, (System.DateTime.Today - StartDate).Days);
        // Computes the unique hashed playerID based on deviceidentifier suffix-salted
        int seed;
        using (SHA256 sha256 = SHA256.Create())
        {
            sha256.Initialize();
            byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier + "gametunut"));
            seed = (int)ConvertLittleEndian(hashValue);
            playerID = Convert.ToBase64String(hashValue);
            playerIDShort = playerID.Substring(0, 6);
            sha256.Clear();
        }
        // Computes random class assigements from playerhash
        if (CurentTestClass == TestClass.Undefined)
        {
            UnityEngine.Random.InitState(seed);
            CurentTestClass = (TestClass)UnityEngine.Random.Range(0, 8);
        }
        else
        {
            Debug.Log("Test class assignement skipped, overriden by a class set in unity. Please set to UNDEFINED before release and for pseudorandom assignement using device ID.");
        }
        // Logging
        Debug.Log("[Global info setup]"
            + "\n Current day : " + System.DateTime.Today + " / Start day : " + StartDate
            + "\nDAY:" + CurrentDay + " / PlayerID:" + playerID + " (" + playerIDShort + ") / TestClass : " + CurentTestClass
            + "\n Goal : " + hasGoal() + " - Awards : " + hasAwards() + " - Storytelling : " + hasStorytelling());
    }


    private bool globalSetup = false;
    /// <summary>
    /// First game update post start setup
    /// </summary>
    public void Update()
    {
        if (!globalSetup)
        {
            globalSetup = true;
            // Start page mover
            switch (CurentTestClass)
            {
                case TestClass.Class_Goal:
                    ViewManager.Instance.OnViewChange(GameView.Messages);
                    break;
                case TestClass.Class_Awards:
                    ViewManager.Instance.OnViewChange(GameView.Profil);
                    break;
                case TestClass.Class_Storytelling:
                    ViewManager.Instance.OnViewChange(GameView.Menu);
                    break;
                case TestClass.Class_GoalAwards:
                    ViewManager.Instance.OnViewChange(GameView.Profil);
                    break;
                case TestClass.Class_AwardsStorytelling:
                    ViewManager.Instance.OnViewChange(GameView.Profil);
                    break;
                case TestClass.Class_GoalStorytelling:
                    ViewManager.Instance.OnViewChange(GameView.Menu);
                    break;
                case TestClass.Class_All:
                    ViewManager.Instance.OnViewChange(GameView.Menu);
                    break;
                case TestClass.Undefined:
                default:
                    Debug.LogError("TestClass was undefined or outside of possible range. This is an error and should be fixed.");
                    break;
            }
        }
    }

    /// <summary>
    /// https://stackoverflow.com/questions/6165171/convert-byte-array-to-int
    /// </summary>
    private ulong ConvertLittleEndian(byte[] array)
    {
        int pos = 0;
        ulong result = 0;
        foreach (byte by in array)
        {
            result |= ((ulong)by) << pos;
            pos += 8;
        }
        return result;
    }

    public enum TestClass
    {
        Class_Goal = 0, Class_Awards = 1, Class_Storytelling = 2, Class_GoalAwards = 3, Class_AwardsStorytelling = 4, Class_GoalStorytelling = 5, Class_All = 6, Undefined = 999
    }

    /// <returns>True if the current TestClass has the Goal mechanic</returns>
    public bool hasGoal()
    {
        return CurentTestClass == TestClass.Class_Goal || CurentTestClass == TestClass.Class_GoalAwards || CurentTestClass == TestClass.Class_GoalStorytelling || CurentTestClass == TestClass.Class_All;
    }

    /// <returns>True if the current TestClass has the awards mechanic</returns>
    public bool hasAwards()
    {
        return CurentTestClass == TestClass.Class_Awards || CurentTestClass == TestClass.Class_GoalAwards || CurentTestClass == TestClass.Class_AwardsStorytelling || CurentTestClass == TestClass.Class_All;
    }

    /// <returns>True if the current TestClass has the Storytelling mechanic</returns>
    public bool hasStorytelling()
    {
        return CurentTestClass == TestClass.Class_Storytelling || CurentTestClass == TestClass.Class_GoalStorytelling || CurentTestClass == TestClass.Class_AwardsStorytelling || CurentTestClass == TestClass.Class_All;
    }

    /// <summary>
    /// Sends a pseudoaninimlized log to the server for any action the player makes.
    /// <param name="data">String data, in csv format. Can contain anything and will be posted to the server as a singleline log. Will not be parsed in any case. 
    /// Note that serevr may cull carriage returns for security, but content will be only lightly sanitized and should not be considered safe.</param>
    /// </summary>
    public IEnumerator sendLogToServer(String data)
    {
        string tosend = System.DateTime.UtcNow + "," + playerID + "," + data;
        WWWForm form = new WWWForm();
        form.AddField("source", "gametunut-" + playerID);
        form.AddField("datacontent", tosend);
        using (UnityWebRequest www = UnityWebRequest.Post("https://www.example.com/sendData", form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
        }
    }

}
