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
        for (int i = 0; i < db.Length; i++)
        {
            GameObject post = Instantiate(postPrefab, transform);
            post.GetComponent<PublicationBehavior>().SetFromData(db[i]);
        }
    }

}
