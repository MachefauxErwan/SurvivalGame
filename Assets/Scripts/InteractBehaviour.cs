using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBehaviour : MonoBehaviour
{
    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Inventory inventory;

    [Header("Tools Visuals")]
    [SerializeField]
    private GameObject pickaxeVisual;

    [SerializeField]
    private GameObject axeVisual;

    private Harvestable currentHarvestable;
    private Item currentItem;
    private Tool currentTool;

    private Vector3 spawnItemOffset = new Vector3(0, 0.5f, 0);

   public void DoPickup(Item item)
   {

        currentItem = item;
        if (inventory.IsFull(currentItem.itemData.name))
        {
            Debug.Log("inventory is full, you can't pick up " + item.name);
            Debug.Log( currentItem.itemData.name);
            return;
        }

        playerAnimator.SetTrigger("Pickup");
        playerMoveBehaviour.canMove = false;
        
   }
    IEnumerator BreakHarvestable()
    {
        if(currentHarvestable.disableKinematicOnHarvest)
        {
            Rigidbody rigidbody = currentHarvestable.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(new Vector3(750, 750, 0), ForceMode.Impulse);
        }

        yield return new WaitForSeconds(currentHarvestable.destroyDelay);
        for (int i = 0; i < currentHarvestable.harvestableItems.Length; i++)
        {
            Ressource ressource = currentHarvestable.harvestableItems[i];
            if (Random.Range(1,101) <= ressource.dropChance)
            {
                GameObject instantiatedRessource = Instantiate(ressource.ItemData.prefab);
                instantiatedRessource.transform.position = currentHarvestable.transform.position + spawnItemOffset;

            }
        }

        Destroy(currentHarvestable.gameObject);


    }
    public void DoHarvest(Harvestable harvestable)
    {
        //pickaxeVisual.gameObject.SetActive(true);
        currentTool = harvestable.tool;
        EnabledToolGameObjectFromEnum(currentTool);

        currentHarvestable = harvestable;
        playerAnimator.SetTrigger("Harvest");
        playerMoveBehaviour.canMove = false;

    }

    // executed during the pickup Event
    public void AddItemToInventory()
    {
        // verifier s'il peut etre stacker
        inventory.AddItem(currentItem.itemData);
        Destroy(currentItem.gameObject);
    }

    public void ReEnablePlayerMovement()
    {
        playerMoveBehaviour.canMove = true;
        EnabledToolGameObjectFromEnum(currentTool, false);
    }

    private void EnabledToolGameObjectFromEnum(Tool toolType, bool enabled = true)
    {
        switch (toolType)
        {
            case Tool.Pickaxe:
                pickaxeVisual.SetActive(enabled);
                break;
            case Tool.Axe:
                axeVisual.SetActive(enabled);
                break;
        }
    }
}
