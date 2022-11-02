using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

//Holds reference and count of items, manages their visibility in the Inventory panel
public class CraftSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item = null;

    [SerializeField]
    private TMPro.TextMeshProUGUI descriptionText;
    [SerializeField]
    private TMPro.TextMeshProUGUI nameText;
    [SerializeField]
    private GameObject CraftingParent;
    [SerializeField]
    private GameObject InventoryParent;


    public List<ItemSlot> inventorySlots = new List<ItemSlot>();
    List<CraftSlot> craftSlots = new List<CraftSlot>();
    public List<CraftResultSlot> craftResultSlots = new List<CraftResultSlot>();

    [SerializeField]
    public Image itemIcon;

    // Start is called before the first frame update
    void Start()
    {
        UpdateGraphic();

        craftSlots = new List<CraftSlot>(
            CraftingParent.GetComponentsInChildren<CraftSlot>()
            );

        craftResultSlots = new List<CraftResultSlot>(
          CraftingParent.GetComponentsInChildren<CraftResultSlot>()
          );

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
        if (item != null)
        {
            descriptionText.text = item.description;
            nameText.text = item.name;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            descriptionText.text = "";
            nameText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ClearCraftSlot();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ClearCraftSlot();
            
        }
    }

    public void ClearCraftSlot()
    {
         item = null;
         itemIcon.sprite = null;
         itemIcon.gameObject.SetActive(false);

        craftResultSlots[0].item = null;
        craftResultSlots[0].itemIcon.sprite = null;
        craftResultSlots[0].itemIcon.gameObject.SetActive(false);

      
        inventorySlots[0].checkForCrafting();

    }

    public void ClearAllCrafting()
    {
        for (int i = 0; i < craftSlots.Count; i++)
        {
            if (craftSlots[i].item != null)
            {
                craftSlots[i].item = null;
                craftSlots[i].itemIcon.sprite = null;
                craftSlots[i].itemIcon.gameObject.SetActive(false);
            }

            craftResultSlots[0].item = null;
            craftResultSlots[0].itemIcon.sprite = null;
            craftResultSlots[0].itemIcon.gameObject.SetActive(false);

            //ItemSlot.newMaterialChecker[i] = false;

        }
    }
}
