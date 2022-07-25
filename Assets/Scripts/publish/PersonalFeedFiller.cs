using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Filler utility to 
/// </summary>
public class PersonalFeedFiller : MonoBehaviour
{

    public GameObject viewContent;

    public GameObject postPrefab;

    public void Start()
    {
        updateContentPosts();
    }

    public void updateContentPosts()
    {
        // Culls existing content, which also shoves everything to the bottom.
        foreach (Transform child in viewContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        // recovers data and populates the list from the bottom up
        string[] data = PlayerPrefs.GetString("plates", "").Split("|");
        if (data != null && data.Length >= 1 && !data[0].Equals(""))
        {
            Debug.Log("Adding " + data.Length + " Personal post to display viewport");
            for (int i = 0; i < data.Length; i++)
            {
                PlateInfo localeinfo = JsonUtility.FromJson<PlateInfo>(data[i]);
                GameObject localePostInstance = Instantiate(postPrefab, viewContent.transform);
                Publication parsedData = new Publication();
                parsedData.posterName = "Le michou";
                parsedData.description = data[i];
                parsedData.likeText = "Aimé par GtunutBot et 4 autres personnes";
                parsedData.comments = "GtunutBot: Super plat, vos abonnés vont adorer!";
                parsedData.profilePath = "selfPostIcon";
                parsedData.imagePath = "placeholder";
                localePostInstance.GetComponent<PublicationBehavior>().SetFromData(parsedData);
            }
        }
        else
        {
            Debug.Log("No data found for personal post, try making a new plate.");
        }

    }
}
