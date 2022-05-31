using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryButton : MonoBehaviour
{

    public string CategoryName = "Default category name";

    public void OnClick() {
        // loads the buttons for the different info panels of that category in the sub1 Pannel
        
        
        // Calls the manager to change UI elements visibility
        ViewManager.Instance.OnViewChange(GameView.InfoSub1);
    }

}
