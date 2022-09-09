using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicPostsManager : MonoBehaviour
{

    public GameObject postPrefab;

    void Start()
    {
        Publication[] db = PublicationDatabase.get();
        // For each day in the experiment, add posts matching that day to the list
        for (int i = GlobalManager.Instance.CurrentDay; i >= 0; i--)
            for (int j = db.Length - 1; j >= 0; j--)
                if (db[j].day == i)
                {
                    GameObject post = Instantiate(postPrefab, transform);
                    post.GetComponent<PublicationBehavior>().SetFromData(db[j]);
                }
    }

}
