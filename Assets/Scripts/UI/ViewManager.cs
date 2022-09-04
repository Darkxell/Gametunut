using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton holding information about the 
/// </summary>
public class ViewManager : MonoBehaviour
{

    public static ViewManager Instance;

    [HideInInspector]
    public GameView currentView = GameView.Menu;

    /// <summary>
    /// Reference to the Masking selector transform, to manage the swiping animation when changing pages.
    /// </summary>
    public RectTransform maskingSelector;

    /// <summary>
    /// Reference to the UI buttons, even when hidden
    /// </summary>
    public GameObject Button_Menu, Button_Profil, Button_Compose, Button_Info;

    /// <summary>
    /// Reference to the red ball object
    /// </summary>
    public GameObject BallReference;
    private float BallGrowthPerSec = 3f, BalllMaxSize = 1f, BallSize = 0f;

    private float currentXanimator = 0, xTargetDefault = -200, xTargetCurrent = -200;

    /// <summary>
    /// List of gameobjects that can be activated or deactivated depending on the test class.
    /// </summary>
    public List<GameObject> classHandles;

    /// <summary>
    /// Reference to the message Button, allowing access to its custom handle behavior
    /// </summary>
    public GameObject MessagesButtonReference;

    /// <returns>A gameobject of the footer button for that page, if available.</returns>
    public GameObject IDtoButtonOject(GameView id)
    {
        switch (id)
        {
            case GameView.Menu:
                return Button_Menu;
            case GameView.Profil:
                return Button_Profil;
            case GameView.Compose:
                return Button_Compose;
            case GameView.Info:
                return Button_Info;
            default:
                return null;
        }
    }

    public void Awake()
    {
        Instance = this;
    }

    public void FixedUpdate()
    {
        // No idea why this gets triggered, but in case it does, thgis fixes a bug where the ball gets stuck on the left side of the screen
        if (xTargetCurrent == 0) {
            GameObject button = IDtoButtonOject(currentView);
            xTargetCurrent = (button && button.activeInHierarchy) ? button.transform.position.x : xTargetDefault;
        }

        maskingSelector.position = new Vector3(currentXanimator, maskingSelector.position.y, 0);
        float epsilon = 1f;
        if (currentXanimator > xTargetCurrent + epsilon || currentXanimator < xTargetCurrent - epsilon)
        {
            float difference = (xTargetCurrent - currentXanimator) / 3f;
            currentXanimator += difference;
        }
        else
        {
            currentXanimator = xTargetCurrent;
        }

        BallSize += Time.fixedDeltaTime * BallGrowthPerSec;
        if (BallSize >= BalllMaxSize)
            BallSize = BalllMaxSize;
        BallReference.transform.localScale = new Vector3(BallSize, BallSize, BallSize);
    }

    /// <summary>
    /// Event called on and to view change, to set the visibility and activity of all game components depending on the selected view.
    /// This also may call specific reset function when pages are left or joined for gameplay purposes.
    /// </summary>
    public void OnViewChange(GameView target)
    {
        currentView = target;
        // Local footer UI changes
        BallReference.transform.localScale = Vector3.zero;
        BallSize = 0f;
        Debug.Log("Changing view to : " + target);
        GameObject button = IDtoButtonOject(target);
        xTargetCurrent = (button && button.activeInHierarchy) ? button.transform.position.x : xTargetDefault;
        // Global UI swap notification
        foreach (var item in PageElement.elements)
        {
            if (item.GameViewType == (int)target)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
        // Messages button notice to recompute its test case handle
        MessagesButtonReference.GetComponent<MessagesButtonBehavior>().updateVisibility();
        GlobalManager.Instance.sendLogToServer("changepage," + target);
    }

    public void OnViewChange(int target)
    {
        OnViewChange((GameView)target);
    }

    /// <summary>
    /// Notices and updates any registered handles so that they can show or hide according to the current testclass.<br>
    /// This is mostly pointless, unless you change class at runtime.
    /// </summary>
    public void updateRegisteredHandles()
    {
        classHandles.ForEach(n => n.GetComponent<TestCaseHandle>().updateVisibility());
    }

}

/// <summary>
/// List of View IDs used by the ViewManager and components ot determine their visibility.
/// Note that info is not technically a page, but a toggle overlay.
/// </summary>
public enum GameView
{
    Menu = 0, Profil = 1, Compose = 2, Info = 3, InfoSub1 = 4, InfoSub2 = 5, Messages = 6, MessageInner = 7, Credits = 10, Undefined = 99
}
