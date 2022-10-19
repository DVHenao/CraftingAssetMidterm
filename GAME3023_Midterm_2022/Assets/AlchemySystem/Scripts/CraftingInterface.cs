using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInterface : MonoBehaviour
{
    List<ItemSlot> itemSlots = new List<ItemSlot>();

    [SerializeField]
    GameObject craftingPanel;

    void Start()
    {
        //Read all itemSlots as children of inventory panel
        itemSlots = new List<ItemSlot>(
            craftingPanel.GetComponentsInChildren<ItemSlot>()
            );
    }
}
