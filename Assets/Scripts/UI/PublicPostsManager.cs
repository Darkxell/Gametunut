using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicPostsManager : MonoBehaviour
{

    public GameObject postPrefab;

    void Start()
    {
        // TODO : check publication date here to not flood everything from the start
        Publication[] db = PublicationDatabase.get();
        for (int i = db.Length - 1; i >= 0; i--)
        {
            GameObject post = Instantiate(postPrefab, transform);
            post.GetComponent<PublicationBehavior>().SetFromData(db[i]);
        }
    }

}
