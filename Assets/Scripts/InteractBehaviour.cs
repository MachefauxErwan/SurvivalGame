using System.Collections;
using UnityEngine;
using System.Linq;

public class InteractBehaviour : MonoBehaviour
{
    [Header("Referencies")]
    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Equipment equipmentSystem;

    [SerializeField]
    private EquipmentLibrary equipmentLibrary;

    [SerializeField]
    private AudioSource audioSource;

    [Header("Tools Configuration")]
    [SerializeField]
    private GameObject pickaxeVisual;

    [SerializeField]
    private AudioClip pickaxeSound;

    [SerializeField]
    private GameObject axeVisual;

    [SerializeField]
    private AudioClip axeSound;


    [HideInInspector]
    public bool isBusy = false;
    public bool canUsePickaxe = false;
    public bool canUseAxe = false;

    [Header("Other")]
    [SerializeField]
   private AudioClip pickupSound;

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
        Harvestable currentlyHarvesting = currentHarvestable;

        currentlyHarvesting.gameObject.layer = LayerMask.NameToLayer("Default");

        if(currentlyHarvesting.disableKinematicOnHarvest)
        {
            Rigidbody rigidbody = currentlyHarvesting.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(transform.forward * 800, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(currentlyHarvesting.destroyDelay);
        for (int i = 0; i < currentlyHarvesting.harvestableItems.Length; i++)
        {
            Ressource ressource = currentlyHarvesting.harvestableItems[i];
            if (Random.Range(1,101) <= ressource.dropChance)
            {
                GameObject instantiatedRessource = Instantiate(ressource.ItemData.prefab);
                instantiatedRessource.transform.position = currentlyHarvesting.transform.position + spawnItemOffset;

            }
        }

        Destroy(currentlyHarvesting.gameObject);


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
        audioSource.PlayOneShot(pickupSound);
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
            EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == equipmentSystem.equipedWeaponItem).FirstOrDefault();

            if (equipmentLibraryItem != null)
            {
                for (int i = 0; i < equipmentLibraryItem.elementToDisable.Length; i++)
                {
                    equipmentLibraryItem.elementToDisable[i].SetActive(enabled);
                }

                equipmentLibraryItem.prefab.SetActive(!enabled);
            }

       
        switch (toolType)
        {
            case Tool.Pickaxe:
                pickaxeVisual.SetActive(enabled);
                audioSource.clip = pickaxeSound;
                break;
            case Tool.Axe:
                axeVisual.SetActive(enabled);
                audioSource.clip = axeSound;
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

    public void playHarvestableSoundEffect()
    {
        audioSource.Play();
    }
}
