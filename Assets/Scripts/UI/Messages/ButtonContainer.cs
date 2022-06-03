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

        for (int i = 0; i < db.Length; i++)
        {
            if (/*check date of db[i] here*/ true)
            {
                GameObject instanceLocale = Instantiate(messageButtonPrefab, transform);

                instanceLocale.GetComponent<MessageButton>().ChangeText(db[i].textlong);
                instanceLocale.GetComponent<MessageButton>().ChangeTextHeader(db[i].sender);
                instanceLocale.GetComponent<MessageButton>().ChangeImage(db[i].picture);

            }
        }
    }

}
