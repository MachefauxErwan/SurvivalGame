using UnityEngine;

public class ItemActionsSystem : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private Equipment equipment;

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
    public InventoryItem itemCurrentlySelected;

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
        equipment.EquipAction();
    }

    public void DropActionButton()
    {
        GameObject InstantiatedItem = Instantiate(itemCurrentlySelected.itemData.prefab);
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
