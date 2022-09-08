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


    public bool canUsePickaxe = false;
    public bool canUseAxe = false;

    private bool isBusy = false; 

    private Harvestable currentHarvestable;
    private Item currentItem;
    private Tool currentTool;

    private Vector3 spawnItemOffset = new Vector3(0, 0.5f, 0);

   public void DoPickup(Item item)
   {
        if(isBusy)
        {
            return;
        }

        isBusy = true;
        currentItem = item;
        if (inventory.IsFull())
        {
            Debug.Log("inventory is full, you can't pick up " + item.name);
            Debug.Log(currentItem.itemData.name);
            return;
        }

        playerAnimator.SetTrigger("Pickup");
        playerMoveBehaviour.canMove = false;
        
   }
    IEnumerator BreakHarvestable()
    {
        currentHarvestable.gameObject.layer = LayerMask.NameToLayer("Default");

        if(currentHarvestable.disableKinematicOnHarvest)
        {
            Rigidbody rigidbody = currentHarvestable.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(transform.forward * 800, ForceMode.Impulse);
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
        if (isBusy)
        {
            return;
        }

        isBusy = true;

        currentTool = harvestable.tool;
        bool doAction = false; 
        switch (currentTool)
        {
            case Tool.Axe:
                doAction = canUsePickaxe;
                break;
            case Tool.Pickaxe:
                doAction = canUseAxe;
                break;
        }

        if (true)
        {
            
            EnabledToolGameObjectFromEnum(currentTool);

            currentHarvestable = harvestable;
            playerAnimator.SetTrigger("Harvest");
            playerMoveBehaviour.canMove = false;
        }
        else{
            Debug.Log(harvestable.tool.ToString() + " don't create ! ");
        }
       

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
        isBusy = false;
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

    public void DoPlantSeed(ItemData itemToSeed)
    {
        playerAnimator.SetTrigger("Plant");
        playerMoveBehaviour.canMove = false;
        Debug.Log("player plant " + itemToSeed.ItemName);
        GameObject instantiatedRessource = Instantiate(itemToSeed.prefab);
        instantiatedRessource.transform.position = this.transform.position + transform.forward;
    }
}
