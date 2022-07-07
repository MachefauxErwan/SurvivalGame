using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{

    [SerializeField]
    private float pickupRange = 2.6f;

    public PickupBehaviour playerPickupBehavior;

    [SerializeField]
    private GameObject PickupText;

    [SerializeField]
    private LayerMask layerMask;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward,out hit, pickupRange, layerMask))
        {
            if(hit.transform.CompareTag("Item"))
            {
                Debug.Log("there is an item front of us");
                PickupText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerPickupBehavior.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }
        }
        else
        {
            PickupText.SetActive(false);
        }
    }
}
