using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ItemActionsSystem : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private PlayerStats playerStats;

    public InteractBehaviour playerInteractBehavior;

    [Header("ITEM ACTIONS SYSTEM VARIABLES")]

    public GameObject actionPanel;

    [SerializeField]
    private GameObject useItemButton;

    [SerializeField]
    private GameObject equipItemButton;

    [SerializeField]
    private GameObject dropItemButton;

    [SerializeField]
    private GameObject destroyItemButton;

    [SerializeField]
    private Transform dropPoint;

    [HideInInspector]
    public ItemData itemCurrentlySelected;

    public void OpenActionPanel(ItemData item, Vector3 slotPosition)
    {
        itemCurrentlySelected = item;

        if (item == null)
        {
            actionPanel.SetActive(false);
            return;
        }

        switch (item.itemType)
        {
            case ItemType.Ressource:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(false);
                break;
            case ItemType.Equipment:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(true);
                break;
            case ItemType.Consumable:
                useItemButton.SetActive(true);
                equipItemButton.SetActive(false);
                break;
        }

        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }

    public void CloseActionPanel()
    {
        actionPanel.SetActive(false);
        itemCurrentlySelected = null;
    }
    public void UseActionButton()
    {
        if (itemCurrentlySelected.ItemName.Contains("Seed"))
       {
            print("Plant : " + itemCurrentlySelected.name);
            Inventory.instance.RemoveItem(itemCurrentlySelected);
            Inventory.instance.RefreshContent();
            playerInteractBehavior.DoPlantSeed(itemCurrentlySelected);
       }
       else
       {
            print("use of : " + itemCurrentlySelected.ItemName);
            playerStats.ConsumeItem(itemCurrentlySelected.healthEffect, itemCurrentlySelected.hungerEffect, itemCurrentlySelected.thirstEffect);
            Inventory.instance.RemoveItem(itemCurrentlySelected);
        }

        CloseActionPanel();
    }

    public void EquipActionButton()
    {
        equipment.EquipAction();
    }

    public void DropActionButton()
    {
        GameObject InstantiatedItem = Instantiate(itemCurrentlySelected.prefab);
        InstantiatedItem.transform.position = dropPoint.position;

        Inventory.instance.RemoveItem(itemCurrentlySelected);
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }
    public void DestroyActionButton()
    {
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }
}
