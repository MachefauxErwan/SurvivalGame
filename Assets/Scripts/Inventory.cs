using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> content = new List<ItemData>();

    [SerializeField]
    private List<int> countStackContent = new List<int>();

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

    private ItemData itemCurrentlySelected;

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
            countStackContent[IdStackingSlot]++;
            IdStackingSlot = -1;
        }
        else
        {
            content.Add(item);
            countStackContent.Add(1);
           
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
            currentSlot.itemVisual.sprite = content[i].visual;
            currentSlot.itemStack.text = ""+countStackContent[i];
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
            if (content[i].name == itemName)
            {
                // Verifier le stack Item 
                if (content[i].maximumStacking > countStackContent[i])
                {
                    Debug.Log("le slot " + i + " est disponible");
                    return i;
                }
                    
            }
        }
        return -1;
    }
     
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
        print("use item : " + itemCurrentlySelected.name);
        CloseActionPanel();
    }
    public void EquipActionButton()
    {
        print("equip item : " + itemCurrentlySelected.name);
        CloseActionPanel();
    }
    public void DropActionButton()
    {
        GameObject InstantiatedItem = Instantiate(itemCurrentlySelected.prefab);
        InstantiatedItem.transform.position = dropPoint.position;

        int idSlot = GetIndexStakableSlot(itemCurrentlySelected.name);

        //verifier le stack
        if (countStackContent[idSlot] >1)
        {
            countStackContent[idSlot]--;
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
        content.Remove(itemCurrentlySelected);
        RefreshContent();
        CloseActionPanel();
    }


}
