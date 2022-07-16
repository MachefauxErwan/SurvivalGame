using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item",menuName ="Items/New item")]
public class ItemData : ScriptableObject
{
    public string name;
    public Sprite visual;
    public GameObject prefab;
    public string inventoryDescription;
    public int maximumStacking;
    public ItemType itemType;
}

public enum ItemType
{
    Ressource,
    Equipment,
    Consumable
}