using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior description for items that can be activated/deactivated by the viewmanager depending on the Test class
/// </summary>
public class TestCaseHandle : MonoBehaviour
{
    void Start()
    {
        ViewManager.Instance.classHandles.Add(gameObject);
        registered = true;
        updateVisibility();
    }

    private bool registered = false;

    public bool ShowOnClass_Goal;
    public bool ShowOnClass_Awards;
    public bool ShowOnClass_Storytelling;
    public bool ShowOnClass_GoalAwards;
    public bool ShowOnClass_AwardsStorytelling;
    public bool ShowOnClass_GoalStorytelling;
    public bool ShowOnClass_All;

    public bool ShouldShowOn(GlobalManager.TestClass testclass)
    {
        switch (testclass)
        {
            case GlobalManager.TestClass.Class_Goal:
                return ShowOnClass_Goal;
            case GlobalManager.TestClass.Class_Awards:
                return ShowOnClass_Awards;
            case GlobalManager.TestClass.Class_Storytelling:
                return ShowOnClass_Storytelling;
            case GlobalManager.TestClass.Class_GoalAwards:
                return ShowOnClass_GoalAwards;
            case GlobalManager.TestClass.Class_AwardsStorytelling:
                return ShowOnClass_AwardsStorytelling;
            case GlobalManager.TestClass.Class_GoalStorytelling:
                return ShowOnClass_GoalStorytelling;
            case GlobalManager.TestClass.Class_All:
                return ShowOnClass_All;
            case GlobalManager.TestClass.Undefined:
                return ShowOnClass_Goal;
        }
        return true;
    }

    public void updateVisibility()
    {
        gameObject.SetActive(ShouldShowOn(GlobalManager.Instance.CurentTestClass));
    }

}
