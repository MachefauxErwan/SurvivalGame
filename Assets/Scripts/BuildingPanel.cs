using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanel : MonoBehaviour
{
    [SerializeField]
    private BuildSystem buildSystem;
    // Start is called before the first frame update
    [SerializeField]
    private List<StructureData> buildingStructureContent = new List<StructureData>();
    [SerializeField]
    GameObject StructureSlotPrefabs;
    [SerializeField]
    Transform StructureSlotParent;

    [Header("UI Ressources")]
    [SerializeField]
    private Transform builSystemRessourcePanel;
    [SerializeField]
    private GameObject buildingRequiredElements;


    [SerializeField]
    private GameObject buildingPanel;
    private bool isOpen = false;
    private StructureData currentStructure;
    private int currentStructureIndex;
    private List<BluidingSlot> listStructureSlot = new List<BluidingSlot>();

    private void Start()
    {

        currentStructure = buildingStructureContent[0];
        currentStructureIndex = 0; 
        ClosePanel();

    }
    private void LoadStructuresSlot()
    {
        listStructureSlot.Clear();
        for (int i = 0; i < StructureSlotParent.childCount; i++)
        {
            Destroy(StructureSlotParent.gameObject.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < buildingStructureContent.Count; i++)
        {
            GameObject StructureSlotObj = Instantiate(StructureSlotPrefabs, StructureSlotParent);
            BluidingSlot bluidingSlotInstance = StructureSlotObj.GetComponent<BluidingSlot>();
            bluidingSlotInstance.Configure(buildingStructureContent[i], this);
            listStructureSlot.Add(bluidingSlotInstance);
        }
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildingPanel.SetActive(!buildingPanel.activeSelf);
            if (isOpen)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();

            }
        }
        if(isOpen)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if(currentStructureIndex > 0)
                {
                    currentStructureIndex--;
                    changeStructureType(buildingStructureContent[currentStructureIndex]);
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (currentStructureIndex < (buildingStructureContent.Count-1))
                {
                    currentStructureIndex++;
                    changeStructureType(buildingStructureContent[currentStructureIndex]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                changeStructureType(buildingStructureContent[0]);
                
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                changeStructureType(buildingStructureContent[1]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                changeStructureType(buildingStructureContent[2]);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                buildSystem.RotateStructure();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                buildSystem.BuildStructure();
            }
        }
    }

    public void changeStructureType(StructureData structureData)
    {
        currentStructure = structureData;
        buildSystem.GetStructureByType(currentStructure.structureType);
        UpdateDisplayedCost();
        foreach (var structure in listStructureSlot)
        {
            structure.onSelected(false);
        }
        listStructureSlot[currentStructureIndex].onSelected(true);
    }

    public void UpdateDisplayedCost()
    {
        foreach (Transform child in builSystemRessourcePanel)
        {
            Destroy(child.gameObject);
        }
        foreach (ItemInInventory requiredRessource in currentStructure.requiredItems)
        {
            GameObject RequiredElementGO = Instantiate(buildingRequiredElements, builSystemRessourcePanel);
            RequiredElementGO.GetComponent<BuildingRequiredItemSlot>().Setup(requiredRessource);
        }
    }

    private void OpenPanel()
    {
        buildingPanel.SetActive(true);
        buildSystem.EnableBuildSystem();
        LoadStructuresSlot();
        isOpen = true;
        changeStructureType(buildingStructureContent[0]);
    }
    public void ClosePanel()
    {
        buildingPanel.SetActive(false);
        buildSystem.DisableBuildSystem();
        isOpen = false;
        
    }
}
