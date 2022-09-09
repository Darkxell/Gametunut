using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simplified monobehavior for viewchange trigger buttons, such as "close" or "back" ones.
/// </summary>
public class ViewChangeTrigger : MonoBehaviour
{

    public void changeView(int gameView)
    {
        if (ViewManager.Instance == null)
        {
            Debug.LogError("Fatal : ViewManager does not exist or has been culled, Game view may not be changed anymore.");
            return;
        }
        // Dogshit code, but simplest solution to not have back buttons move you out of bounds
        int changeto = gameView;
        if (GlobalManager.Instance.CurentTestClass == GlobalManager.TestClass.Class_Awards
            || GlobalManager.Instance.CurentTestClass == GlobalManager.TestClass.Class_GoalAwards)
        {
            if (gameView == (int)GameView.Menu) changeto = (int)GameView.Profil;
        }
        if (GlobalManager.Instance.CurentTestClass == GlobalManager.TestClass.Class_Goal)
        {
            if (gameView == (int)GameView.Menu || gameView == (int)GameView.Profil) changeto = (int)GameView.Messages;
        }
        ViewManager.Instance.OnViewChange(changeto);
    }

}
