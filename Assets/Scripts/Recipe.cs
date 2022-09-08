using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Recipe : MonoBehaviour
{
    

    private RecipeData currentRecipe;

    [SerializeField]
    private Image craftableItemImage;

    [SerializeField]
    private GameObject elementRequiredPrefab;

    [SerializeField]
    private Transform elementRequiredParent;

    [SerializeField]
    private Button craftButton;

    [SerializeField]
    private Sprite canBuildIcon;

    [SerializeField]
    private Sprite cantBuildIcon;

    [SerializeField]
    private Color missingColor;

    [SerializeField]
    private Color availableColor;

    public void Configure(RecipeData recipe)
    {
        currentRecipe = recipe;
        craftableItemImage.sprite = recipe.craftableItem.visual;
        craftableItemImage.transform.parent.GetComponent<Slot>().item = recipe.craftableItem;
        bool CanCraft = true;

        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            // Récupère les elemenets nécessaires pour cette recette
            GameObject requiredItemGameObject = Instantiate(elementRequiredPrefab, elementRequiredParent);
            Image requiredItemGameObjectImage = requiredItemGameObject.GetComponent<Image>();
            ItemData requiredItem = recipe.requiredItems[i].itemData;
            ElementRequired elementRequired = requiredItemGameObject.GetComponent<ElementRequired>();

            //Slot qui permet l'affichage du Tooltip Lorsqu'on lui passe un item
            requiredItemGameObject.GetComponent<Slot>().item = requiredItem;

            ItemInInventory[] itemInInventory = Inventory.instance.GetContent().Where(elem => elem.itemData == requiredItem).ToArray();

            int totalRequiredItemInInventory = 0;

            for (int j = 0; j < itemInInventory.Length; j++)
            {
                totalRequiredItemInInventory += itemInInventory[j].count;
            }
           
            //Si a copie de l'inventaire contient l'element requis alors on le retire de l'inventaire et on passe au suivant
            if (totalRequiredItemInInventory >= recipe.requiredItems[i].count)
            {
                requiredItemGameObjectImage.color = availableColor;
            }
            else
            {
                requiredItemGameObjectImage.color = missingColor;
                CanCraft = false;
            }

            //Configure le visuel de l'élément requis
            elementRequired.elementImage.sprite = recipe.requiredItems[i].itemData.visual;
            elementRequired.elementText.text = recipe.requiredItems[i].count.ToString();
        }

        //Gestion de l'affichage du bouton
        craftButton.image.sprite = CanCraft ? canBuildIcon : cantBuildIcon;
        craftButton.enabled = CanCraft;

        ResizeElementRequiredParents();
    }

    private void ResizeElementRequiredParents()
    {
        Canvas.ForceUpdateCanvases();
        elementRequiredParent.GetComponent<ContentSizeFitter>().enabled = false;
        elementRequiredParent.GetComponent<ContentSizeFitter>().enabled = true;
    }

    public void CraftItem()
    {
        for (int i = 0; i < currentRecipe.requiredItems.Length; i++)
        {
            for (int j = 0; j < currentRecipe.requiredItems[i].count; j++)
            {
                Inventory.instance.RemoveItem(currentRecipe.requiredItems[i].itemData);
            }
           
        }

        if(currentRecipe.craftableItem.itemType == ItemType.Tool)
        {
            
            Debug.Log("You can use" + currentRecipe.craftableItem.name);
        }
        else
        {
            Inventory.instance.AddItem(currentRecipe.craftableItem);
        }
       
    }
}
