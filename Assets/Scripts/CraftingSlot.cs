using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour
{
    public ItemData item;

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            TooltipSystem.instance.Show(item.inventoryDescription, item.name, item.maximunDurability);
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
