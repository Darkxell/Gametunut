using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the messages buttons container to generate the sliding content according to game state and database info
/// </summary>
public class ButtonContainer : MonoBehaviour
{
    public static ButtonContainer Instance;

    public GameObject messageButtonPrefab;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MessageInfo[] db = MessageDatabase.getList();
        Debug.Log("Loading messages from database in UI, found " + db.Length + " entries.\nTodays date is " + System.DateTime.Today + " and we are day " + GlobalManager.Instance.CurrentDay + " in the experiment.");
        for (int i = db.Length - 1; i >= 0; i--)
        {
            if (db[i].releasedate <= GlobalManager.Instance.CurrentDay)
            {
                GameObject instanceLocale = Instantiate(messageButtonPrefab, transform);

                string localetextpreview = GlobalManager.Instance.hasStorytelling() ? db[i].textlong : db[i].infos.requete;
                if (localetextpreview.Length >= 115)
                    localetextpreview = localetextpreview.Substring(0, 115) + "...";
                instanceLocale.GetComponent<MessageButton>().ChangeText(localetextpreview);
                instanceLocale.GetComponent<MessageButton>().ChangeTextHeader(db[i].sender);
                instanceLocale.GetComponent<MessageButton>().ChangeImage(db[i].picture);
                instanceLocale.GetComponent<MessageButton>().ChangeNotifIcon(NotifIconType.Seen);

                instanceLocale.GetComponent<MessageButton>().data = db[i];
            }
        }
        updateNotifIcons();
    }

    /// <summary>
    /// Updates the notification icons of all childs
    /// </summary>
    public void updateNotifIcons()
    {
        HashSet<int> completedMissions = new HashSet<int>();
        string workbuffer = PlayerPrefs.GetString("missions", "");
        Debug.Log("Updating notification icons from saved data : " + workbuffer);
        if (!workbuffer.Equals(""))
        {
            string[] buffarray = workbuffer.Split("|");
            for (int i = 0; i < buffarray.Length; i++)
                if (buffarray[i] != "")
                    completedMissions.Add(int.Parse(buffarray[i]));
        }

        foreach (Transform child in transform)
            if (child.GetComponent<MessageButton>() != null)
            {
                MessageButton obj = child.GetComponent<MessageButton>();
                obj.ChangeNotifIcon(completedMissions.Contains(obj.data.id) ? NotifIconType.Completed : NotifIconType.New);
            }
    }

}
