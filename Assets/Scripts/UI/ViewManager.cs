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
    public GameObject Button_Menu, Button_Profil, Button_Compose;

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
            default:
                return null;
        }
    }

    public void Awake()
    {
        Instance = this;
    }

    public void Update()
    {


    }

    /// <summary>
    /// Event called on and to view change, to set the visibility and activity of all game components depending on the selected view.
    /// This also may call specific reset function when pages are left or joined for gameplay purposes.
    /// </summary>
    public void OnViewChange(GameView target)
    {
        Debug.Log("Changing view to : " + target);
        GameObject button = IDtoButtonOject(target);
        if (button && button.activeInHierarchy) {
            Debug.Log("Clicked button position is : " + button.transform.position);
        }

    }

    public void OnViewChange(int target)
    {
        OnViewChange((GameView)target);
    }

}

/// <summary>
/// List of View IDs used by the ViewManager and components ot determine their visibility.
/// Note that info is not technically a page, but a toggle overlay.
/// </summary>
public enum GameView
{
    Menu = 0, Profil = 1, Compose = 2, Credits = 10
}
