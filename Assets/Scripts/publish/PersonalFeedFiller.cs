using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Filler utility to 
/// </summary>
public class PersonalFeedFiller : MonoBehaviour
{

    public static PersonalFeedFiller Instance;

    public GameObject viewContent;
    public GameObject postPrefab;

    private static string[] CommentsArray = new string[]{"C'est super les utilisateurs de l'application vont adorer !",
        "Ce menu risque d'inspirer beaucoup de personnes !",
        "Wouah ce mélange d'aliments !! Tu me surprends de plus en plus !",
        "J'espère en revoir d'autres des menus comme celui-ci !",
        "Je pense que je vais partager ce menu sur le fil d'actualité !",
        "J'ai jamais vu autant de couleur c'est magnifique !",
        "Comment tu fais pour avoir de si bonnes idées !",
        "Si tu proposes les mêmes menus pour les requêtes d'utilisateur je comprends mieux pourquoi il y a autant de demandes !"};

    public void Awake()
    {
        Instance = this;
    }

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
                if (data[i].Equals(""))
                {
                    Debug.Log("Empty plate data, this is a save corruption bug. Ignoring the post in personal feed at position : " + i);
                    continue;
                }
                PlateInfo localeinfo = JsonUtility.FromJson<PlateInfo>(data[i]);
                GameObject localePostInstance = Instantiate(postPrefab, viewContent.transform);
                Publication parsedData = new Publication();
                parsedData.posterName = "KingOfDishes";
                string dataString = "Contient : ";
                try
                {
                    PlateInfo plateInfos = JsonUtility.FromJson<PlateInfo>(data[i]);
                    for (int k = 0; k < plateInfos.content.Count; k++)
                    {
                        if (k > 0) dataString += ", ";
                        dataString += plateInfos.content[k].ingredientID;
                    }
                }
                catch (System.Exception)
                {
                    dataString += "Que des bonnes choses!";
                }
                parsedData.description = dataString;
                parsedData.likeText = "Aimé par GtunutBot et " + (int)(3 + 5* Mathf.Pow(i,0.8f)) +" autres personnes";
                parsedData.day = -1;
                try
                {
                    parsedData.comments = "<b>GtunutBot:</b> " + CommentsArray[i % CommentsArray.Length];
                }
                catch (System.Exception)
                { 
                    parsedData.comments = "<b>GtunutBot:</b> Super plat, vos abonnés vont adorer!";
                }
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
