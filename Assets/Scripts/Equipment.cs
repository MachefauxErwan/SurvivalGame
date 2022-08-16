using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Equipment : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]
    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    [Header("EQUIPMENT SYSTEM VARIABLES")]

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

    //Garde une trace des equipements actuels
    private ItemData equipedHeadItem;
    private ItemData equipedChestItem;
    private ItemData equipedHandsItem;
    private ItemData equipedLegsItem;
    private ItemData equipedFeetsItem;

    [SerializeField]
    private Button headSlotDesequipButton;
    [SerializeField]
    private Button chestSlotDesequipButton;
    [SerializeField]
    private Button handsSlotDesequipButton;
    [SerializeField]
    private Button legsSlotDesequipButton;
    [SerializeField]
    private Button feetsSlotDesequipButton;


    private void DisablePreviousEquipedEquipment(ItemData itemToDisable)
    {
        if (itemToDisable == null)
        {
            return;
        }

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemToDisable).First();

        if (equipmentLibraryItem != null)
        {
            for (int i = 0; i < equipmentLibraryItem.elementToDisable.Length; i++)
            {
                equipmentLibraryItem.elementToDisable[i].SetActive(true);
            }

            equipmentLibraryItem.prefab.SetActive(false);
        }

        Inventory.instance.AddItem(itemToDisable);
    }

    public void DesequipEquipment(EquipmentType equipmentType)
    {
        if (Inventory.instance.IsFull())
        {
            Debug.Log("Impossible de se déséquipé de cet élément, l'inventaire est plein");
            return;
        }

        ItemData currentItem = null;

        switch (equipmentType)
        {
            case EquipmentType.None:
                break;
            case EquipmentType.Head:
                currentItem = equipedHeadItem;
                equipedHeadItem = null;
                headSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Chest:
                currentItem = equipedChestItem;
                equipedChestItem = null;
                chestSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Hands:
                currentItem = equipedHandsItem;
                equipedHandsItem = null;
                handsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Legs:
                currentItem = equipedLegsItem;
                equipedLegsItem = null;
                legsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Feets:
                currentItem = equipedFeetsItem;
                equipedFeetsItem = null;
                feetSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            default:
                break;
        }


        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == currentItem).First();

        if (equipmentLibraryItem != null)
        {
            for (int i = 0; i < equipmentLibraryItem.elementToDisable.Length; i++)
            {
                equipmentLibraryItem.elementToDisable[i].SetActive(true);
            }

            equipmentLibraryItem.prefab.SetActive(false);
        }

        Inventory.instance.AddItem(currentItem);
        Inventory.instance.RefreshContent();
    }
    public void UpdateEquipmentsDesequipButton()
    {
        headSlotDesequipButton.onClick.RemoveAllListeners();
        headSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Head); });
        headSlotDesequipButton.gameObject.SetActive(equipedHeadItem);

        chestSlotDesequipButton.onClick.RemoveAllListeners();
        chestSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Chest); });
        chestSlotDesequipButton.gameObject.SetActive(equipedChestItem);

        handsSlotDesequipButton.onClick.RemoveAllListeners();
        handsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Hands); });
        handsSlotDesequipButton.gameObject.SetActive(equipedHandsItem);

        legsSlotDesequipButton.onClick.RemoveAllListeners();
        legsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Legs); });
        legsSlotDesequipButton.gameObject.SetActive(equipedLegsItem);

        feetsSlotDesequipButton.onClick.RemoveAllListeners();
        feetsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Feets); });
        feetsSlotDesequipButton.gameObject.SetActive(equipedFeetsItem);
    }

    public void EquipAction()
    {
        print("equip item : " + itemActionsSystem.itemCurrentlySelected.itemData.name);

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemActionsSystem.itemCurrentlySelected.itemData).First();

        if (equipmentLibraryItem != null)
        {

            switch (itemActionsSystem.itemCurrentlySelected.itemData.equipmentType)
            {
                case EquipmentType.Head:
                    DisablePreviousEquipedEquipment(equipedHeadItem);
                    headSlotImage.sprite = itemActionsSystem.itemCurrentlySelected.itemData.visual;
                    equipedHeadItem = itemActionsSystem.itemCurrentlySelected.itemData;
                    break;
                case EquipmentType.Chest:
                    DisablePreviousEquipedEquipment(equipedChestItem);
                    chestSlotImage.sprite = itemActionsSystem.itemCurrentlySelected.itemData.visual;
                    equipedChestItem = itemActionsSystem.itemCurrentlySelected.itemData;
                    break;
                case EquipmentType.Hands:
                    DisablePreviousEquipedEquipment(equipedHandsItem);
                    handsSlotImage.sprite = itemActionsSystem.itemCurrentlySelected.itemData.visual;
                    equipedHandsItem = itemActionsSystem.itemCurrentlySelected.itemData;
                    break;
                case EquipmentType.Legs:
                    DisablePreviousEquipedEquipment(equipedLegsItem);
                    legsSlotImage.sprite = itemActionsSystem.itemCurrentlySelected.itemData.visual;
                    equipedLegsItem = itemActionsSystem.itemCurrentlySelected.itemData;
                    break;
                case EquipmentType.Feets:
                    DisablePreviousEquipedEquipment(equipedFeetsItem);
                    feetSlotImage.sprite = itemActionsSystem.itemCurrentlySelected.itemData.visual;
                    equipedFeetsItem = itemActionsSystem.itemCurrentlySelected.itemData;
                    break;

            }

            for (int i = 0; i < equipmentLibraryItem.elementToDisable.Length; i++)
            {
                equipmentLibraryItem.elementToDisable[i].SetActive(false);
            }
            equipmentLibraryItem.prefab.SetActive(true);

            Inventory.instance.RemoveItem(itemActionsSystem.itemCurrentlySelected);
        }
        else
        {
            Debug.LogError("Equipement :" + itemActionsSystem.itemCurrentlySelected.itemData.name + " n'est pas présent dans la libraries des equipements");
        }

        itemActionsSystem.CloseActionPanel();
    }
}
