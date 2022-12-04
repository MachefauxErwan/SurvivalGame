using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    [SerializeField]
    private float interactRange = 2.6f;

    public InteractBehaviour playerInteractBehavior;

    [SerializeField]
    private GameObject InteractText;

    [SerializeField]
    private LayerMask layerMask;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward,out hit, interactRange, layerMask))
        {
            InteractText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.transform.CompareTag("Item"))
                {
                    playerInteractBehavior.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
                if (hit.transform.CompareTag("Harvestable"))
                {
                    Debug.Log("On a interagi avec l'objet :"+hit.transform.name);
                    playerInteractBehavior.DoHarvest(hit.transform.gameObject.GetComponent<Harvestable>());
                }

            }

           
        }
        else
        {
            InteractText.SetActive(false);
        }
    }
}
