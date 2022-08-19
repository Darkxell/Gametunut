using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoriesList : MonoBehaviour, DragCallable
{
    /// <summary>
    /// List of ingredients categories prefabs in the scroll list
    /// </summary>
    [HideInInspector]
    public List<GameObject> categories = new List<GameObject>(25);

    /// <summary>
    /// Category item prefab for display and mechanical purposes
    /// </summary>
    [SerializeField]
    private GameObject CategoryPrefab;

    /// <summary>
    /// Ingredient item prefab for display and mechanical purposes
    /// </summary>
    [SerializeField]
    private GameObject IngredientPrefab;

    /// <summary>
    /// Sub ingredients list, aka the wheel containing the drag/droppable elements on the right
    /// </summary>
    [SerializeField]
    private GameObject sublist;

    private float scroll = 0f;
    private readonly static float unitHeight = 0.8f;

    // Half horizontal size of the cull drag ui box, in unity world unit
    private float dragBoxSizeX = 0.4f;
    // Half vertical size of the cull drag ui box, in unity world unit
    private float dragBoxSizeY = 2f;

    private static float snapCooldown = 0.10f;
    private float snapCooldownCurrent = 0f;

    /// <summary>
    /// Last index used to set child ingredients, to not set them multiple times.
    /// </summary>
    private int lastSubSet = -1;
    private int selectedID;

    void Start()
    {
        DragDetector.lastInstance.callbacks.Add(this);
        List<string> keys = CategoryManager.getKeySet();
        for (int i = 0; i < keys.Count; i++)
        {
            GameObject localechild = Instantiate(CategoryPrefab, transform);
            CategoryBehavior behavior = localechild.GetComponent<CategoryBehavior>();
            behavior.data = CategoryManager.getDataFor(keys[i]);
            categories.Add(localechild);
            behavior.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("composer/categories/" + behavior.data.sprite);
        }
    }

    void Update()
    {
        RebaseElements();
        snapCooldownCurrent -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        selectedID = (int)((scroll + unitHeight / 2f) / unitHeight);
        float autoscrollspeed = 0.05f;
        // If no scroll for a while, snap scroll to nearest element index.
        if (snapCooldownCurrent <= 0)
        {
            float scrolloffset = scroll % unitHeight;
            if (Mathf.Abs(scrolloffset) < autoscrollspeed * 2)
            {
                if (scrolloffset <= unitHeight / 2)
                    if (Mathf.Abs(scrolloffset) > 0.001f) scroll -= scrolloffset;
                    else if (Mathf.Abs(scrolloffset) < unitHeight - 0.001f) scroll += scrolloffset;
            }
            else
            {
                scroll += (scrolloffset >= unitHeight / 2) ? autoscrollspeed : -autoscrollspeed;
            }

        }
        // If outside array range, force go back in it.
        float maxscroll = (categories.Count - 1) * unitHeight;
        if (scroll < 0) scroll = 0;
        else if (scroll > maxscroll) scroll = maxscroll;
        // Sets the sub ingredients list to the right content, if needed.
        // But does not do it while scrolling.
        if (snapCooldownCurrent <= 0 && selectedID != lastSubSet)
        {
            lastSubSet = selectedID;
            SubIngredientsList slist = sublist.GetComponent<SubIngredientsList>();
            if (slist == null)
            {
                Debug.LogError("Sub ingredient list reference is broken, or does not have a behavior element. Cannot set ingredients to display.");
            }
            else
            {
                CategoryData currentSelected = categories[selectedID].GetComponent<CategoryBehavior>().data;
                Debug.Log("Loading ingredients from new selected category : " + currentSelected.name + " containing " + currentSelected.ingredients.Length + " entries...");
                slist.ingredients = new List<GameObject>(25);
                foreach (Transform child in sublist.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                for (int i = 0; i < currentSelected.ingredients.Length; i++)
                {
                    if (!BattlePassManager.Instance.ingredientsBlacklist.Contains(currentSelected.ingredients[i]))
                    {
                        GameObject localechild = Instantiate(IngredientPrefab, sublist.transform);
                        IngredientBehavior behavior = localechild.GetComponent<IngredientBehavior>();
                        behavior.data = IngredientsManager.getDataFor(currentSelected.ingredients[i]);
                        slist.ingredients.Add(localechild);
                        behavior.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("composer/ingredients/" + behavior.data.spriteMenu);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sets the positions of list elements according to the scroll value
    /// </summary>
    private void RebaseElements()
    {
        for (int i = 0; i < categories.Count; i++)
        {
            categories[i].gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - (i * unitHeight) + scroll, 0);
        }
    }

    public void OnDrag(Vector2 Position, Vector2 force, int actionID)
    {
        bool inside = Position.x < transform.position.x + dragBoxSizeX &&
            Position.x > transform.position.x - dragBoxSizeX &&
            Position.y < transform.position.y + dragBoxSizeY &&
            Position.y > transform.position.y - dragBoxSizeY;
        if (inside)
        {
            snapCooldownCurrent = snapCooldown;
            scroll += force.y;
        }
    }

    void OnDestroy()
    {
        // Unnessessary, but cleaner.
        DragDetector.lastInstance.callbacks.Remove(this);
    }
}
