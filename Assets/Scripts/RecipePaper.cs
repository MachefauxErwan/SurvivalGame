using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePaper : MonoBehaviour
{

    [SerializeField]
    RecipeData recipeToAdd;

    [SerializeField]
    RecipeData[] recipesNeedeed;
    // Start is called before the first frame update
    [SerializeField]
    CraftingSystem craftingSystem;

    void AddRecipeInCraftableBook()
    {
        craftingSystem.AddRecipes(recipeToAdd);
        Destroy(gameObject);
    }

    private void Start()
    {
        AddRecipeInCraftableBook();
    }

}
