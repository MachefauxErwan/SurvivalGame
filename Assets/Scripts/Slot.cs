using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;
    public Image itemVisual;
    public Text itemStack;

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
        {
            TooltipSystem.instance.Show(item.itemData.inventoryDescription, item.itemData.name, item.itemData.maximunDurability);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.instance.Hide();
    }

    public void ClickOnSlot()
    {
        itemActionsSystem.OpenActionPanel(item, transform.position);
    }

}
