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

    
}
