using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Inventory inventory;

    private Item currentItem;
   public void DoPickup(Item item)
   {

        currentItem = item;
        if (inventory.IsFull(currentItem.itemData.name))
        {
            Debug.Log("inventory is full, you can't pick up " + item.name);
            Debug.Log( currentItem.itemData.name);
            return;
        }

        


        //if (inventory.IsStakable(item.itemData.maximumStacking,))

        playerAnimator.SetTrigger("Pickup");
        playerMoveBehaviour.canMove = false;
        
   }
     
    // executed during the pickup Event
    public void AddItemToInventory()
    {
        // verifier s'il peut etre stacker
        inventory.AddItem(currentItem.itemData);
        Destroy(currentItem.gameObject);
        currentItem = null;
    }

    public void ReEnablePlayerMovement()
    {
        playerMoveBehaviour.canMove = true;
    }
}
