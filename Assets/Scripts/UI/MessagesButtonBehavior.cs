using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior to hide the messages button when the current pannel shouldn't have a messages button.<br>
/// Extends the TestCaseHandler to hide access to the messages pannel whgenever the current test class makes it unavailable
/// </summary>
public class MessagesButtonBehavior : TestCaseHandle
{

    public override bool ShouldShowOn(GlobalManager.TestClass testclass)
    {
        if (!base.ShouldShowOn(testclass)) return false;

        switch (ViewManager.Instance.currentView)
        {
            case GameView.Undefined:
            case GameView.Menu:
            case GameView.Profil:
                return true;
            case GameView.Compose:
            case GameView.Info:
            case GameView.InfoSub1:
            case GameView.InfoSub2:
            case GameView.Messages:
            case GameView.MessageInner:
            case GameView.Credits:
                return false;
        }
        return true;
    }




}
