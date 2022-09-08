using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{

    [SerializeField]
    private List<RecipeData> availableRecipes;

    [SerializeField]
    private GameObject recipeUIPrefab;

    [SerializeField]
    private Transform recipeParent;

    [SerializeField]
    private KeyCode openCraftPanelImput;

    [SerializeField]
    private GameObject craftingPanel;

    // Update is called once per frame
    void Start()
    {
        UpdateDisplayedRecipes();
    }

    private void Update()
    {
        if(Input.GetKeyDown(openCraftPanelImput))
        {
            craftingPanel.SetActive(!craftingPanel.activeSelf);
            UpdateDisplayedRecipes();
        }
    }

    public void UpdateDisplayedRecipes()
    {
        foreach(Transform child in recipeParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < availableRecipes.Count; i++)
        {
           GameObject recipe = Instantiate(recipeUIPrefab, recipeParent);
            recipe.GetComponent<Recipe>().Configure(availableRecipes[i]);
        }
    }

    public void AddRecipes(RecipeData recipeDataToAdd)
    {
        availableRecipes.Add(recipeDataToAdd);
    }
}
