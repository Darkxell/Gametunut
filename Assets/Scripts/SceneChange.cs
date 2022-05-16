using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    public void LoadScene(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }

}
