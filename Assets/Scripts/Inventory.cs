using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    [SerializeField]
    private CraftingSystem craftingSystem;

    [SerializeField]
    private BuildingPanel BuildingPanel;

    [Header("INVENTORY SYSTEM VARIABLES")]

    public static Inventory instance;
    private bool isOpen = false;
    private Slot currentSlot;


    [SerializeField]
    private List<ItemInInventory> content = new List<ItemInInventory>();

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

        // recuperer son slot 
        ItemInInventory[] itemInInventory = content.Where(elem => elem.itemData == item).ToArray();

        bool itemAdded = false;

        if(itemInInventory.Length>0 && item.stackable)
        {
            for (int i = 0; i < itemInInventory.Length; i++)
            {
                if(itemInInventory[i].count < item.maximumStacking)
                {
                    itemAdded = true;
                    itemInInventory[i].count++;
                    break;
                }
            }

            if(!itemAdded)
            {
                content.Add(new ItemInInventory
                {
                    itemData = item,
                    count = 1
                });
            }
        }
        else
        {
            content.Add(new ItemInInventory
            {
                itemData = item,
                count = 1
            });
            
        }
        RefreshContent();

    }
    public void RemoveItem(ItemData item)
    {

        ItemInInventory itemInInventory = content.Where(elem => elem.itemData == item).FirstOrDefault();

        if(itemInInventory != null && itemInInventory.count > 1 )
        {
            itemInInventory.count--;
        }
        else
        {
            content.Remove(itemInInventory);
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
                    currentSlot.item = content[i].itemData;
                    currentSlot.itemVisual.sprite = content[i].itemData.visual;

                    if(currentSlot.item.stackable)
                    {
                        currentSlot.countText.enabled = true;
                        currentSlot.countText.text = content[i].count.ToString();
                    }
                }
                break;
        }

        equipment.UpdateEquipmentsDesequipButton();
        craftingSystem.UpdateDisplayedRecipes();
        BuildingPanel.UpdateDisplayedCost();
    }

    private void ClearVisualContent()
    {
        // On vide tous les slots / visuel
        for (int i = 0; i < inventorySlotParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySlotVisual;
            currentSlot.countText.enabled = false;
            currentSlot.countText.text = "1";
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
                currentSlot.item = content[i].itemData;
                currentSlot.itemVisual.sprite = content[i].itemData.visual;

                if (currentSlot.item.stackable)
                {
                    currentSlot.countText.enabled = true;
                    currentSlot.countText.text = content[i].count.ToString();
                }
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
                currentSlot.item = content[i].itemData;
                currentSlot.itemVisual.sprite = content[i].itemData.visual;

                if (currentSlot.item.stackable)
                {
                    currentSlot.countText.enabled = true;
                    currentSlot.countText.text = content[i].count.ToString();
                }
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
                currentSlot.item = content[i].itemData;
                currentSlot.itemVisual.sprite = content[i].itemData.visual;
                if (currentSlot.item.stackable)
                {
                    currentSlot.countText.enabled = true;
                    currentSlot.countText.text = content[i].count.ToString();
                }
                idSlotFilter++;
            }
        }
    }

    public List<ItemInInventory> GetContent()
    {
        return content;
    }

    public bool IsFull()
    {
        return (InventorySize == content.Count);
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

    public bool OnCheckElementIsInInventory(ItemInInventory itemRequired)
    {
        ItemInInventory[] itemInInventory = GetContent().Where(elem => elem.itemData == itemRequired.itemData).ToArray();
        int totalRequiredItemQuantityInventory = 0;
        for (int i = 0; i < itemInInventory.Length; i++)
        {
            totalRequiredItemQuantityInventory += itemInInventory[i].count;
        }
        if (totalRequiredItemQuantityInventory >= itemRequired.count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

[System.Serializable]
public class ItemInInventory
{
    public ItemData itemData;
    public int count;
    
}

public enum InventoryFilter
{
    All,
    Ressource,
    Equipment,
    Consumable
}
