using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Back button method holder for the information pannels, since they have different behavior
/// </summary>
public class InfoBackButton : MonoBehaviour
{

    /// <summary>
    /// Gameview to go to when clicking the close info button
    /// </summary>
    public static GameView backanchor = GameView.Menu;

    /// <summary>
    /// Method called each page change to store the fallback of a potential info panel opening. Doesn't necessarly change the anchor the the parsed value, if at all.
    /// </summary>
    /// <param name="hint">contains the page changed to each time is changed.</param>
    public static void setBackAnchor(GameView hint)
    {
        switch (hint)
        {
            case GameView.Menu:
                backanchor = GameView.Menu;
                break;
            case GameView.Profil:
                backanchor = GameView.Profil;
                break;
            case GameView.Compose:
                backanchor = GameView.Compose;
                break;
            case GameView.Messages:
                backanchor = GameView.Messages;
                break;
            case GameView.MessageInner:
                backanchor = GameView.MessageInner;
                break;
            case GameView.Credits:
            case GameView.Info:
            case GameView.InfoSub1:
            case GameView.InfoSub2:
                return;
            case GameView.Undefined:
            default:
                backanchor = GameView.Menu;
                break;
        }
    }

    public void onClick()
    {
        Debug.Log("Exited info pannel, changing view back to : " + backanchor);
        ViewManager.Instance.OnViewChange(backanchor);
    }



}
