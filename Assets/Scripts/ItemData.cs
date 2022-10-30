using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item",menuName ="Items/New item")]
public class ItemData : ScriptableObject
{
    [Header("Data")]
    public string ItemName;
    public Sprite visual;
    public GameObject prefab;
    public string inventoryDescription;
    public bool stackable;
    public int maximumStacking;
    public int maximunDurability;
   
    [Header("Effect")]
    public float healthEffect;
    public float hungerEffect;
    public float thirstEffect;

    [Header("Armor Stats")]
    public float armorPoints;

    [Header("Attack Stats")]
    public float AttackPoints;

    [Header("Type")]
    public ItemType itemType;
    public EquipmentType equipmentType;
}

public enum ItemType
{
    Ressource,
    Equipment,
    Consumable,
    Tool
}

public enum EquipmentType
{
    None,
    Head,
    Chest,
    Hands,
    Legs,
    Feets,
    Weapon
}