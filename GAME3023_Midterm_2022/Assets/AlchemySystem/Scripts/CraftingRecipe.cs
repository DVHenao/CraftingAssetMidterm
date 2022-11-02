using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct ItemAmount
{

    public Item item;
    [Range(1,999)]
    public int amount;

}

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public List<Item> Materials;
    public List<Item> Results;

}
