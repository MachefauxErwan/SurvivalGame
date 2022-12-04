using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class BuildingRequiredItemSlot : MonoBehaviour
{
    [Header("UI elements required")]
    [SerializeField]
    private Text elementCount;
    [SerializeField]
    private Image elementImage;

    [SerializeField]
    private Image slotImage;

    [SerializeField]
    private Color greenColor = Color.green;
    [SerializeField]
    private Color redColor = Color.red;

    public bool hasRessouces = false;

    public void Setup(ItemInInventory itemRequired)
    {
        elementCount.text = itemRequired.count.ToString();
        elementImage.sprite = itemRequired.itemData.visual;
        if (Inventory.instance.OnCheckElementIsInInventory(itemRequired))
        {
            hasRessouces= true;
            slotImage.color = greenColor;
        }
        else
        {
            slotImage.color = redColor;
        }
    }

    

}
