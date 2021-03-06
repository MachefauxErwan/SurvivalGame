using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Inventory : MonoBehaviour
{
    [Header("Inventory Panel Referencies")]

    public static Inventory instance;
    private bool isOpen = false;

    [SerializeField]
    private List<InventoryItem> content = new List<InventoryItem>();

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private Transform inventorySlotParent;

    const int InventorySize = 24;

    private int IdStackingSlot;
    
    [SerializeField]
    private Transform dropPoint;

    [Header("Action Panel Referencies")]

    [SerializeField]
    private GameObject actionPanel;

    [SerializeField]
    private GameObject useItemButton;

    [SerializeField]
    private GameObject equipItemButton;

    [SerializeField]
    private GameObject dropItemButton;
    
    [SerializeField]
    private GameObject destroyItemButton;

    private InventoryItem itemCurrentlySelected;

    [SerializeField]
    private Sprite emptySlotVisual;

    [Header("Equipement Panel Referencies")]

    [SerializeField]
    private EquipmentLibrary equipmentLibrary;
    
    [SerializeField]
    private Image headSlotImage;

    [SerializeField]
    private Image chestSlotImage;
    
    [SerializeField]
    private Image handsSlotImage;
    
    [SerializeField]
    private Image legsSlotImage;
    
    [SerializeField]
    private Image feetSlotImage;
 
    [Header("Filter Panel Referencies")]
    
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
        RefreshContent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            if(isOpen)
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
    private void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        isOpen = true;
    }
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        actionPanel.SetActive(false);
        TooltipSystem.instance.Hide();
        currentFilter = InventoryFilter.All;
        isOpen = false;
    }

    private void RefreshContent()
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
                // On peuple le visuel des slots selon le r?el de l'inventaire
                FilterVisualRessource();
                break;
            case InventoryFilter.All:
            default:

                // On peuple le visuel des slots selon le r?el de l'inventaire
                for (int i = 0; i < content.Count; i++)
                {
                    Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
                    currentSlot.item = content[i];
                    currentSlot.itemVisual.sprite = content[i].itemData.visual;
                    currentSlot.itemStack.text = "" + content[i].itemStack;
                }
                break;
        }
       
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
        // On peuple le visuel des slots selon le r?el de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {
            
            if(content[i].itemData.itemType == ItemType.Equipment)
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
        // On peuple le visuel des slots selon le r?el de l'inventaire
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
        // On peuple le visuel des slots selon le r?el de l'inventaire
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
   

    public bool IsFull(string itemName)
    {
        //verifier stack
        bool isStakable = IsStakable(itemName);
        if (isStakable)
        {
            return false;
        }
        return (InventorySize == content.Count);
    }

    public int GetIndexStakableSlot (string itemName)
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

    public void OpenActionPanel(InventoryItem item, Vector3 slotPosition)
    {
        itemCurrentlySelected = item;

        if (item == null)
        {
            actionPanel.SetActive(false);
            return;
        }

        switch (item.itemData.itemType)
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
        print("use item : " + itemCurrentlySelected.itemData.name);
        CloseActionPanel();
    }
    public void EquipActionButton()
    {
        print("equip item : " + itemCurrentlySelected.itemData.name);

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemCurrentlySelected.itemData).First();

        if(equipmentLibraryItem !=null)
        {
            for (int i = 0; i < equipmentLibraryItem.elementToDisable.Length; i++)
            {
                equipmentLibraryItem.elementToDisable[i].SetActive(false);
            }

            equipmentLibraryItem.prefab.SetActive(true);
            switch(itemCurrentlySelected.itemData.equipmentType)
            {
                case EquipmentType.Head :
                    headSlotImage.sprite = itemCurrentlySelected.itemData.visual;
                    break;
                case EquipmentType.Chest:
                    chestSlotImage.sprite = itemCurrentlySelected.itemData.visual;
                    break;
                case EquipmentType.Hands:
                    handsSlotImage.sprite = itemCurrentlySelected.itemData.visual;
                    break;
                case EquipmentType.Legs:
                    legsSlotImage.sprite = itemCurrentlySelected.itemData.visual;
                    break;
                case EquipmentType.feets:
                    feetSlotImage.sprite = itemCurrentlySelected.itemData.visual;
                    break;

            }

        }
        else
        {
            Debug.LogError("Equipement :" + itemCurrentlySelected.itemData.name + " n'est pas pr?sent dans la libraries des equipements");
        }

        CloseActionPanel();
    }
    public void DropActionButton()
    {
        GameObject InstantiatedItem = Instantiate(itemCurrentlySelected.itemData.prefab);
        InstantiatedItem.transform.position = dropPoint.position;

        int idSlot = itemCurrentlySelected.IDSlot;
        if (content[idSlot].itemStack > 1)
        {
            content[idSlot].itemStack--;
        }
        else
        {
            //content[idSlot].itemStack = 0;
            content.Remove(itemCurrentlySelected);
        }

        RefreshContent();
        CloseActionPanel();
    }
    public void DestroyActionButton()
    {
        int idSlot = itemCurrentlySelected.IDSlot;
        if (content[idSlot].itemStack > 1)
        {
            content[idSlot].itemStack--;
        }
        else
        {
            content.Remove(itemCurrentlySelected);
        }
        RefreshContent();
        CloseActionPanel();
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
