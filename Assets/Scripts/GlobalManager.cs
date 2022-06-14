using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

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
        UnityEngine.Random.InitState(seed);
        CurentTestClass = (TestClass)UnityEngine.Random.Range(0, 8);
        // Logging
        Debug.Log("[Global info setup]"
            + "\n Current day : " + System.DateTime.Today + " / Start day : " + StartDate
            + "\nDAY:" + CurrentDay + " / PlayerID:" + playerID + " (" + playerIDShort + ") / TestClass : " + CurentTestClass
            + "\n Goal : " + hasGoal() + " - Awards : " + hasAwards() + " - Storytelling : " + hasStorytelling());
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
}
