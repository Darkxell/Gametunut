using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the messages buttons container to generate the sliding content according to game state and database info
/// </summary>
public class ButtonContainer : MonoBehaviour
{

    public GameObject messageButtonPrefab;

    void Start()
    {
        MessageInfo[] db = MessageDatabase.getList();
        Debug.Log("Loading messages from database in UI, found " + db.Length + " entries.\nTodays date is " + System.DateTime.Today + " and we are day " + GlobalManager.Instance.CurrentDay + " in the experiment.");
        for (int i = 0; i < db.Length; i++)
        {
            if (db[i].releasedate <= GlobalManager.Instance.CurrentDay)
            {
                GameObject instanceLocale = Instantiate(messageButtonPrefab, transform);

                instanceLocale.GetComponent<MessageButton>().ChangeText(db[i].textlong);
                instanceLocale.GetComponent<MessageButton>().ChangeTextHeader(db[i].sender);
                instanceLocale.GetComponent<MessageButton>().ChangeImage(db[i].picture);
                instanceLocale.GetComponent<MessageButton>().ChangeNotifIcon(NotifIconType.Seen);

                instanceLocale.GetComponent<MessageButton>().data = db[i];
            }
        }
    }

}
