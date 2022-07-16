using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<InventoryItem> content = new List<InventoryItem>();

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private Transform inventorySlotParent;

    const int InventorySize = 24;

    private int IdStackingSlot;

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

    [SerializeField]
    private Transform dropPoint;

    public static Inventory instance;

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
        }
    }

    public void AddItem(ItemData item)
    {
       // IdStackingSlot = GetIndexStakableSlot(item.name);
        if (IdStackingSlot != -1)
        {
            content[IdStackingSlot].itemStack++;
            //countStackContent[IdStackingSlot]++;
            IdStackingSlot = -1;
        }
        else
        {

            // ajout d'un nouveau element
            InventoryItem newInventoryItem = new InventoryItem();
            newInventoryItem.itemData = item;
            newInventoryItem.itemStack = 1;
            content.Add(newInventoryItem);
            //countStackContent.Add(1);
           
        }
        RefreshContent();

    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    private void RefreshContent()
    {
        // On vide tous les slots / visuel
        for (int i = 0; i < inventorySlotParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySlotVisual;
            currentSlot.itemStack.text = "0";
        }

        // On peuple le visuel des slots selon le réel de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = content[i];
            currentSlot.itemVisual.sprite = content[i].itemData.visual;
            currentSlot.itemStack.text = ""+ content[i].itemStack;
        }
    }


    public bool IsFull(string itemName)
    {
        //verifier stack
        IdStackingSlot = GetIndexStakableSlot(itemName);
        if (IdStackingSlot != -1)
        {
            return false;
        }
        return InventorySize == content.Count;
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
        CloseActionPanel();
    }
    public void DropActionButton()
    {
        GameObject InstantiatedItem = Instantiate(itemCurrentlySelected.itemData.prefab);
        InstantiatedItem.transform.position = dropPoint.position;

        int idSlot = GetIndexStakableSlot(itemCurrentlySelected.itemData.name);

        //verifier le stack
        if (content[idSlot].itemStack >1)
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
    public void DestroyActionButton()
    {
        int idSlot = GetIndexStakableSlot(itemCurrentlySelected.itemData.name);
        if (content[idSlot].itemStack > 1)
        {
            content[idSlot].itemStack--;
        }
        else
        {
            content.Remove(itemCurrentlySelected);
        }
        //content.Remove(itemCurrentlySelected);
        RefreshContent();
        CloseActionPanel();
    }


}

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int itemStack;

}
