using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildSystem : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Structure[] structures;

    [SerializeField]
    private Material blueMaterial;

    [SerializeField]
    private Material redMaterial;

    [SerializeField]
    private Transform rotationRef;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip buildingSound;

    private Structure currentStructure;

    private bool inPlace;
    private bool canBuild;
    private Vector3 finalPosition;
    private bool systemEnabled = false;

    private void Awake()
    {
        currentStructure = structures[0];
    }

    private void FixedUpdate()
    {
        if(!systemEnabled)
        {
            return;
        }

        canBuild = currentStructure.placementPrefabs.GetComponentInChildren<CollisionDetectionEdge>().CheckConnection();
        finalPosition = grid.GetNearestPointOnGrid(transform.position);
        CheckPosition();
        RoundPlacementRotation();
        UpdatePlacementStructureMaterial();
    }

    public void EnableBuildSystem()
    {
        systemEnabled = true;
        //currentStructure.placementPrefabs.SetActive(true);
    }
    public void DisableBuildSystem()
    {
        systemEnabled = false;
        currentStructure.placementPrefabs.SetActive(false);
    }

    public void BuildStructure()
    {
        if(canBuild && inPlace && systemEnabled && hasAllRessources())
        {
            Instantiate(currentStructure.structureData.InstantiatedPrefab,
                currentStructure.placementPrefabs.transform.position,
                currentStructure.placementPrefabs.transform.GetChild(0).transform.rotation);

            audioSource.PlayOneShot(buildingSound);

            ItemInInventory[] requiredItems = currentStructure.structureData.requiredItems;
            for (int i = 0; i < requiredItems.Length; i++)
            {
                for (int j = 0; j < requiredItems[i].count; j++)
                {
                    Inventory.instance.RemoveItem(requiredItems[i].itemData);
                }
            }
        }
    }

    public bool hasAllRessources()
    {
        BuildingRequiredItemSlot[] requiredItem = GameObject.FindObjectsOfType<BuildingRequiredItemSlot>();
        return requiredItem.All(requiredItem => requiredItem.hasRessouces);
    }

    void RoundPlacementRotation()
    {
        float Yangle = rotationRef.localEulerAngles.y;
        int roundedRotation =0;

        if(Yangle > -45 && Yangle <=45)
        {
            roundedRotation = 0;
        }
        else if(Yangle > 45 && Yangle <= 135)
        {
            roundedRotation = 90;
        }
        else if (Yangle > 135 && Yangle <= 225)
        {
            roundedRotation = 180;
        }
        else if (Yangle > 225 && Yangle <= 315)
        {
            roundedRotation = 270;
        }

        currentStructure.placementPrefabs.transform.rotation = Quaternion.Euler(0, roundedRotation, 0);
    }
    public void RotateStructure()
    {
        if(currentStructure.structureData.structureType !=StructureType.Wall)
        {
            currentStructure.placementPrefabs.transform.GetChild(0).transform.Rotate(0, 90, 0);
        }
    }

    private void UpdatePlacementStructureMaterial()
    {
        MeshRenderer placementPrefabRenderer = currentStructure.placementPrefabs.GetComponentInChildren<CollisionDetectionEdge>().meshRenderer;
        if(inPlace && canBuild && hasAllRessources())
        {
            placementPrefabRenderer.material = blueMaterial;
        }
        else
        {
            placementPrefabRenderer.material = redMaterial;
        }
    }

    void CheckPosition()
    {
        inPlace = currentStructure.placementPrefabs.transform.position == finalPosition;

        if(!inPlace)
        {
            SetPosition(finalPosition);
        }
    }

    void SetPosition(Vector3 targetPosition)
    {
        Transform placementPrefabTransform = currentStructure.placementPrefabs.transform;
        Vector3 positionVelocity = Vector3.zero;

        if(Vector3.Distance(placementPrefabTransform.position,targetPosition)>10)
        {
            placementPrefabTransform.position = targetPosition;
        }
        else
        {
            Vector3 newTargetPosition = Vector3.SmoothDamp(placementPrefabTransform.position, targetPosition, ref positionVelocity, 0, 15000);
            placementPrefabTransform.position = newTargetPosition;
        }
    }
    
    public void GetStructureByType(StructureType structureType)
    {
        systemEnabled = true;
        currentStructure =  structures.Where(elem => elem.structureData.structureType == structureType).FirstOrDefault();
        foreach (var structure in structures)
        {
            structure.placementPrefabs.SetActive(structure.structureData.structureType == currentStructure.structureData.structureType);
        }
    }
    
   

    public Structure GetCurrentStructure()
    {
       return currentStructure;
    }
    public bool GetStatusBuildSystem()
    {
        return systemEnabled;
    }

}

[System.Serializable]
public class Structure
{
    public GameObject placementPrefabs;
    public StructureData structureData;
}
