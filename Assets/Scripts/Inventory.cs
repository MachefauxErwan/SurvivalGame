using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    [Header("INVENTORY SYSTEM VARIABLES")]

    public static Inventory instance;
    private bool isOpen = false;

    [SerializeField]
    private List<InventoryItem> content = new List<InventoryItem>();

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private Transform inventorySlotParent;

    const int InventorySize = 24;

    public Sprite emptySlotVisual;

    [Header("FILTER SYSTEM VARIABLES")]

    private InventoryFilter currentFilter = InventoryFilter.All;

    [SerializeField]
    private GameObject inventoryFilterButton;

    [SerializeField]
    private GameObject ressourcesFilterButton;

    [SerializeField]
    private GameObject equipmentsFilterButton;

    [SerializeField]
    private GameObject consumablesFilterButton;

    [SerializeField]
    private Color32 selectedFilterColorSlot;

    [SerializeField]
    private Color32 filterColorSlot;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CloseInventory();
        RefreshContent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            if (isOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    public void AddItem(ItemData item)
    {

        int idSlot = GetIndexStakableSlot(item.name);
        if (idSlot != -1)
        {
            content[idSlot].itemStack++;
            // IdStackingSlot = -1;
        }
        else
        {

            // ajout d'un nouveau element
            InventoryItem newInventoryItem = new InventoryItem();
            newInventoryItem.itemData = item;
            newInventoryItem.itemStack = 1;
            newInventoryItem.IDSlot = content.Count;
            content.Add(newInventoryItem);

        }
        RefreshContent();

    }
    public void RemoveItem(InventoryItem item)
    {       
        if (item.itemStack >= 2)
        {
            item.itemStack--;
        }
        else
        {
            content.Remove(item);

        }
        RefreshContent();
    }

    private void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        isOpen = true;
    }
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        itemActionsSystem.actionPanel.SetActive(false);
        TooltipSystem.instance.Hide();
        currentFilter = InventoryFilter.All;
        isOpen = false;
    }

    public void RefreshContent()
    {
        // On vide tous les slots / visuel
        ClearVisualContent();

        switch (currentFilter)
        {
            case InventoryFilter.Consumable:
                FilterVisualConsumable();
                break;
            case InventoryFilter.Equipment:
                FilterVisualEquipment();
                break;
            case InventoryFilter.Ressource:
                FilterVisualRessource();
                break;
            case InventoryFilter.All:
            default:

                // On peuple le visuel des slots selon le réel de l'inventaire
                for (int i = 0; i < content.Count; i++)
                {
                    Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
                    currentSlot.item = content[i];
                    currentSlot.itemVisual.sprite = content[i].itemData.visual;
                    currentSlot.itemStack.text = "" + content[i].itemStack;
                }
                break;
        }

        equipment.UpdateEquipmentsDesequipButton();
    }

    private void ClearVisualContent()
    {
        // On vide tous les slots / visuel
        for (int i = 0; i < inventorySlotParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySlotVisual;
            currentSlot.itemStack.text = "0";
        }

    }

    private void FilterVisualEquipment()
    {
        int idSlotFilter = 0;
        // On peuple le visuel des slots selon le réel de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {

            if (content[i].itemData.itemType == ItemType.Equipment)
            {
                Slot currentSlot = inventorySlotParent.GetChild(idSlotFilter).GetComponent<Slot>();
                currentSlot.item = content[i];
                currentSlot.itemVisual.sprite = content[i].itemData.visual;
                currentSlot.itemStack.text = "" + content[i].itemStack;
                idSlotFilter++;
            }
        }
    }

    private void FilterVisualConsumable()
    {
        int idSlotFilter = 0;
        // On peuple le visuel des slots selon le réel de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {
            if (content[i].itemData.itemType == ItemType.Consumable)
            {
                Slot currentSlot = inventorySlotParent.GetChild(idSlotFilter).GetComponent<Slot>();
                currentSlot.item = content[i];
                currentSlot.itemVisual.sprite = content[i].itemData.visual;
                currentSlot.itemStack.text = "" + content[i].itemStack;
                idSlotFilter++;
            }
        }
    }
    private void FilterVisualRessource()
    {
        int idSlotFilter = 0;
        // On peuple le visuel des slots selon le réel de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {

            if (content[i].itemData.itemType == ItemType.Ressource)
            {
                Slot currentSlot = inventorySlotParent.GetChild(idSlotFilter).GetComponent<Slot>();
                currentSlot.item = content[i];
                currentSlot.itemVisual.sprite = content[i].itemData.visual;
                currentSlot.itemStack.text = "" + content[i].itemStack;
                idSlotFilter++;
            }
        }
    }


    public bool IsFull(string itemName = "")
    {
        //verifier stack
        bool isStakable = IsStakable(itemName);
        if (isStakable)
        {
            return false;
        }
        return (InventorySize == content.Count);
    }

    public int GetIndexStakableSlot(string itemName)
    {
        for (int i = 0; i < content.Count; i++)
        {
            //Verifier le nom Item dans l'inventaire
            if (content[i].itemData.name == itemName)
            {
                // Verifier le stack Item 
                if (content[i].itemData.maximumStacking > content[i].itemStack)
                {
                    Debug.Log("le slot " + i + " est disponible");
                    return i;
                }

            }
        }
        return -1;
    }

    public bool IsStakable(string itemName)
    {
        for (int i = 0; i < content.Count; i++)
        {
            //Verifier le nom Item dans l'inventaire
            if (content[i].itemData.name == itemName)
            {
                // Verifier le stack Item 
                if (content[i].itemData.maximumStacking > content[i].itemStack)
                {
                    Debug.Log("le slot " + i + " est stakable");
                    return true;
                }

            }
        }
        return false;
    }


    public int GetIndexSlotFromItemName(string itemName)
    {
        for (int i = 0; i < content.Count; i++)
        {
            //Verifier le nom Item dans l'inventaire
            if (content[i].itemData.name == itemName)
            {
                // Verifier le stack Item 

                Debug.Log("le slot " + i + " est disponible");
                return i;

            }
        }
        return -1;
    }



    public void InventoryFilterButton()
    {
        inventoryFilterButton.GetComponent<Image>().color = selectedFilterColorSlot;
        equipmentsFilterButton.GetComponent<Image>().color = filterColorSlot;
        ressourcesFilterButton.GetComponent<Image>().color = filterColorSlot;
        consumablesFilterButton.GetComponent<Image>().color = filterColorSlot;
        currentFilter = InventoryFilter.All;

        RefreshContent();
    }
    public void EquipmentFilterButton()
    {
        inventoryFilterButton.GetComponent<Image>().color = filterColorSlot;
        equipmentsFilterButton.GetComponent<Image>().color = selectedFilterColorSlot;
        ressourcesFilterButton.GetComponent<Image>().color = filterColorSlot;
        consumablesFilterButton.GetComponent<Image>().color = filterColorSlot;

        currentFilter = InventoryFilter.Equipment;

        RefreshContent();
    }
    public void RessourceFilterButton()
    {
        inventoryFilterButton.GetComponent<Image>().color = filterColorSlot;
        equipmentsFilterButton.GetComponent<Image>().color = filterColorSlot;
        ressourcesFilterButton.GetComponent<Image>().color = selectedFilterColorSlot;
        consumablesFilterButton.GetComponent<Image>().color = filterColorSlot;

        currentFilter = InventoryFilter.Ressource;

        RefreshContent();
    }
    public void ConsumableFilterButton()
    {
        inventoryFilterButton.GetComponent<Image>().color = filterColorSlot;
        equipmentsFilterButton.GetComponent<Image>().color = filterColorSlot;
        ressourcesFilterButton.GetComponent<Image>().color = filterColorSlot;
        consumablesFilterButton.GetComponent<Image>().color = selectedFilterColorSlot;

        currentFilter = InventoryFilter.Consumable;

        RefreshContent();
    }

}

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int itemStack;
    public int IDSlot;
}

public enum InventoryFilter
{
    All,
    Ressource,
    Equipment,
    Consumable
}
