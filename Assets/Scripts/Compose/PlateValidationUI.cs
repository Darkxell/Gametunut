using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Plate validation ui main component. This is technically not a view type and is part of the compose view, and is a locked standalone with controlled exits.
/// </summary>
public class PlateValidationUI : MonoBehaviour
{

    /// <summary>
    /// Last instance of the plate vlaidation ui to remotely change things on it
    /// </summary>
    public static PlateValidationUI Instance;

    public TextMeshProUGUI GbotTextfield;

    public GameObject unlockpanel;
    public Image unlockImage;

    private bool currentSuccess = false;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private static string trans_empty = "Il semblerait que le plat que vous proposez soit vide. Essayez de glisser-déposer des aliments choisis à gauche dans l'assiette!";
    private static string trans_success = "Félicitations!<br> Le plat que vous avez proposé convient à merveille!";
    private static string trans_failure = "Ce plat ne semble correspondre à la requète de l'expéditeur, essaye de lui proposer un autre menu.";
    private static string trans_unlock = "<br><br>Vous avez débloqué un nouvel aliment!";

    /// <summary>
    /// Changes the content of the plate validation ui given the wanted parametters.
    /// Call this method before setting the view to active
    /// </summary>
    public void ChangeContent(bool questSuccess, bool unlock, string unlockID)
    {
        Debug.Log("Changing content for plate validation confirmation UI : " + questSuccess + " / unlock : " + unlock + " - " + unlockID);
        currentSuccess = questSuccess;
        GbotTextfield.text = (questSuccess ? trans_success : trans_failure) + (unlock ? trans_unlock : "");
        unlockpanel.gameObject.SetActive(unlock);
        if (unlock)
        {
            try
            {
                unlockImage.sprite = Resources.Load<Sprite>("composer/Ingredients/" + IngredientsManager.getDataFor(unlockID).spriteMenu);
                GlobalManager.Instance.sendLogToServer("reward," + unlockID);
            }
            catch (System.Exception)
            {
                Debug.LogError("Cannot set the displayed ingredient award to the right texture, unfound ID for : " + unlockID);
            }
        }
    }

    /// <summary>
    /// Event called when the player presses the validate button at the bottom of the UI
    /// </summary>
    public void onButtonPress()
    {

        if (currentSuccess)
        {
            ViewManager.Instance.OnViewChange(GlobalManager.Instance.CurentTestClass == GlobalManager.TestClass.Class_Goal ? GameView.Messages : GameView.Profil);
        }
        else
        {
            // ViewManager.Instance.OnViewChange(GameView.Compose);
            // Unneeded, since you can't be outside compose when this code is run...?
        }
        gameObject.SetActive(false);
    }

}
