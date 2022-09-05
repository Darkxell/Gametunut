using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Behavior of the inner view of a message, able to manipulate visual components to display custom data
/// </summary>
public class MessageInnerBehavior : MonoBehaviour
{

    public static MessageInnerBehavior Instance;

    public GameObject NameHeader;
    public GameObject ProfilePicture;
    public GameObject RecievedText;
    public GameObject ContentText;
    public GameObject NameSmall;
    public GameObject PictureSmall;

    public MessageInfo lastData = null;

    /// <summary>
    /// Show/hide-able objects based on quest completion 
    /// </summary>
    public GameObject ButtonAccept, ButtonDeny, TextDone;

    private int forceSizeUpdate = 0;

    public void Awake()
    {
        Instance = this;
    }

    public void SetFromData(MessageInfo data)
    {
        lastData = data;
        // Text setters

        NameHeader.GetComponent<TextMeshProUGUI>().text = data.sender;
        RecievedText.GetComponent<TextMeshProUGUI>().text = "Release date : " + data.releasedate;
        NameSmall.GetComponent<TextMeshProUGUI>().text = data.infos.nom;

        // Image setters

        ProfilePicture.GetComponent<Image>().sprite = Resources.Load<Sprite>("profiles/" + data.picture);
        PictureSmall.GetComponent<Image>().sprite = Resources.Load<Sprite>("profiles/" + data.picture);

        string repascomplettext = "<br><br>Un <b>Repas complet</b> nécessite de respecter les quatres jauges.";

        // Text content generator
        if (GlobalManager.Instance.hasStorytelling())
        {
            ContentText.GetComponent<TextMeshProUGUI>().text = data.textlong + (data.respectbars ? repascomplettext : ""); ;
        }
        else
        {
            ContentText.GetComponent<TextMeshProUGUI>().text = "Age : " + data.infos.age + "<br>"
                    + "Taille : " + data.infos.taille + " m<br>"
                    + "Poids : " + data.infos.poids + " kgs<br>"
                    + "IMC : " + data.infos.imc + "<br>"
                    + "Sexe : " + data.infos.sexe + "<br>"
                    + "Problème : " + data.infos.probleme + "<br>"
                    + "Requète : " + data.infos.requete
                    + (data.respectbars ? repascomplettext : "");
        }

        // Button/text enable logic
        string[] completedmissions = PlayerPrefs.GetString("missions", "").Split("|");
        bool missioncomplete = false;
        for (int i = 0; i < completedmissions.Length; i++)
            if (!completedmissions[i].Equals("") && int.Parse(completedmissions[i]) == data.id)
            {
                missioncomplete = true;
                break;
            }
        if (missioncomplete)
        {
            ButtonAccept.SetActive(false);
            ButtonDeny.SetActive(false);
            TextDone.SetActive(true);
        }
        else
        {
            ButtonAccept.SetActive(true);
            ButtonDeny.SetActive(true);
            TextDone.SetActive(false);
        }

        // Bugfix layout srescaler : Schedule an update in 2 frames
        forceSizeUpdate = 2;
    }

    private void Update()
    {
        // Dogshit code because this doesn't work : LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.transform as RectTransform);
        if (forceSizeUpdate >= 0)
            forceSizeUpdate--;
        if (forceSizeUpdate == 1)
        {
            forceSizeUpdate = 0;
            ContentText.gameObject.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
            ContentText.gameObject.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;

            ContentText.gameObject.transform.parent.gameObject.GetComponent<ContentSizeFitter>().enabled = false;
            ContentText.gameObject.transform.parent.gameObject.GetComponent<ContentSizeFitter>().enabled = true;
        }
    }
}
