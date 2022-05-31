using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CategoriesField : MonoBehaviour
{
    public GameObject buttonPrefab;

    void Awake()
    {
        InformationsData data = InformationsData.Get();

        for (int i = 0; i < data.categories.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, transform);
            button.GetComponent<CategoryButton>().CategoryName = data.categories[i];
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.categories[i];
        }

    }
}
