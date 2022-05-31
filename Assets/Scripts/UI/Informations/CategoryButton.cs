using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CategoryButton : MonoBehaviour
{

    public string CategoryName = "Default category name";

    public void OnClick()
    {
        // Deletes previous child buttons
        foreach (Transform child in InfoListContainer.Instance.transform)
            GameObject.Destroy(child.gameObject);
        // Loads the buttons for the different info panels of that category in the sub1 Pannel
        InformationsData data = InformationsData.Get();
        for (int i = 0; i < data.content.Length; i++)
        {
            InformationAsset infopanel = data.content[i];
            if (infopanel.parent.Equals(CategoryName))
            {
                GameObject buttonlocale = Instantiate(InfoListContainer.Instance.InfopanelButtonPrefab, InfoListContainer.Instance.transform);
                buttonlocale.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.content[i].name;

            }
        }

        // Calls the manager to change UI elements visibility
        ViewManager.Instance.OnViewChange(GameView.InfoSub1);



    }

}
