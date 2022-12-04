using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluidingSlot : MonoBehaviour
{


    [Header("UI Structure")]
    [SerializeField]
    private Text structureName;
    [SerializeField]
    private Image structureImage;
    [SerializeField]
    private Image structureImageSelected;



    private StructureData structureData;
    private BuildingPanel buildingPanel;
    public void Configure(StructureData data, BuildingPanel buildPanel)
    {
        structureName.text = data.name;
        structureImage.sprite = data.visual;
        structureData = data;
        buildingPanel = buildPanel;
    }

    public void onSelected(bool EnableStatus)
    {
        if(EnableStatus)
            structureImageSelected.color = new Color(structureImageSelected.color.r, structureImageSelected.color.g, structureImageSelected.color.b, 1 );
        else
            structureImageSelected.color = new Color(structureImageSelected.color.r, structureImageSelected.color.g, structureImageSelected.color.b, 0);
    }

    public void onSelectStructure()
    {
        buildingPanel.changeStructureType(structureData);
    }
}
