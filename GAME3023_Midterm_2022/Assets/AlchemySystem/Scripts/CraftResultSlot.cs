using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

//Holds reference and count of items, manages their visibility in the Inventory panel
public class CraftResultSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item = null;
    public CraftingRecipe craftingRecipeVar = null;

    [SerializeField]
    private GameObject CraftingParent;
    [SerializeField]
    private GameObject InventoryParent;

    public List<CraftSlot> craftSlots = new List<CraftSlot>();
    public List<CraftingRecipe> CRList = new List<CraftingRecipe>();
    CraftingRecipe[] CRArray;
    public List<ItemSlot> inventorySlots = new List<ItemSlot>();

 

    [SerializeField]
    public Image itemIcon;

    // Start is called before the first frame update
    void Start()
    {
        UpdateGraphic();

        craftSlots = new List<CraftSlot>(
            CraftingParent.GetComponentsInChildren<CraftSlot>()
            );

        CRArray = Resources.LoadAll<CraftingRecipe>("Recipes");

        for (int i = 0; i < CRArray.Length; i++)
        {
            CRList.Add(CRArray[i]);
        }

        inventorySlots = new List<ItemSlot>(
            InventoryParent.GetComponentsInChildren<ItemSlot>()
            );
    }

    //Change Icon and count
    void UpdateGraphic()
    {
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //leave empty
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //leave empty
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Craft();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Craft();

        }
    }



    public void Craft()
    {
        if (item != null)
        {
        bool ClearCraftSlot = false;
        int AlreadyInInventoryCounter = 0;
       
            for (int i = 0; i < inventorySlots.Count; i++) // for every inventory slot
            {
                if (inventorySlots[i].item != null) // check to make sure there is an item
                {
                    for (int x = 0; x < craftSlots.Count; x++) // if there is an item, cross reference it with our crafts slots
                    {
                        if (craftSlots[x].item != null) // our craftslots that do have items.
                        {
                            if (craftSlots[x].item == inventorySlots[i].item)// if our craftslot item matches inventory slots item
                            {
                                inventorySlots[i].Count -= 1; // subtract

                                if (inventorySlots[i].Count == 0)
                                {
                                    craftSlots[x].item = null;
                                    craftSlots[x].itemIcon.sprite = null;
                                    craftSlots[x].itemIcon.gameObject.SetActive(false);
                                    ClearCraftSlot = true;
                                }

                            }
                        }
                    }

                }

                if (item == inventorySlots[i].item)
                {
                    inventorySlots[i].Count += 1;
                    AlreadyInInventoryCounter = 0;
                }
                else
                    AlreadyInInventoryCounter += 1;

                if (AlreadyInInventoryCounter == inventorySlots.Count) // there is no preexisting stack of items anywhere
                {
                    for (int x = 0; x < inventorySlots.Count; x++)
                    {
                        if (inventorySlots[x].item == null)
                        {
                            inventorySlots[x].item = item;
                            inventorySlots[x].itemIcon.sprite = item.icon;
                            inventorySlots[x].itemIcon.gameObject.SetActive(true);
                            inventorySlots[x].itemCountText.gameObject.SetActive(true);
                            inventorySlots[x].itemCountText.text = inventorySlots[x].count.ToString();
                            inventorySlots[x].Count = 1;
                            break;
                        }
                    }
                }



            }
            if (ClearCraftSlot == true)
            {
                item = null;
                itemIcon.sprite = null;
                itemIcon.gameObject.SetActive(false);
                ClearCraftSlot = false;
                inventorySlots[0].checkForCrafting();
            }
        }
    }
}
