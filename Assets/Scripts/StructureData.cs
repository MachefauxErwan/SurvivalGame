using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Structure", menuName = "Structure/New Structure")]
public class StructureData : ScriptableObject
{
    [Header("Data")]
    public string StructureName;
    public Sprite visual;
    public GameObject InstantiatedPrefab;
    public ItemInInventory[] requiredItems;

    [Header("Type")]
    public StructureType structureType;
}

public enum StructureType
{
    Wall,
    Stairs,
    Floor,
    Door
}

