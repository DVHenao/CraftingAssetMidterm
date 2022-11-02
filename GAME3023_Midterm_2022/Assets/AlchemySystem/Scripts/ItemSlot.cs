using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Holds reference and count of items, manages their visibility in the Inventory panel
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item = null;

    [SerializeField]
    private TMPro.TextMeshProUGUI descriptionText;
    [SerializeField]
    private TMPro.TextMeshProUGUI nameText;
    [SerializeField]
    private GameObject CraftingParent;
    [SerializeField]
    private GameObject CharacterParent;


    public List<CraftSlot> craftSlots = new List<CraftSlot>();
    public List<CraftResultSlot> craftResultSlots = new List<CraftResultSlot>();
    public List<CharacterStat> CharacterStats = new List<CharacterStat>();

    static public List<CraftingRecipe> CRList = new List<CraftingRecipe>();
    CraftingRecipe[] CRArray;

    [SerializeField]
    public int count = 0;
    public int Count
    {
        get { return count; }
        set
        {
            count = value;
            UpdateGraphic();
        }
    }

    [SerializeField]
    public Image itemIcon;

    [SerializeField]
    public TextMeshProUGUI itemCountText;

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
        CharacterStats = new List<CharacterStat>(
            CharacterParent.GetComponentsInChildren<CharacterStat>()
            );

        CRArray = Resources.LoadAll<CraftingRecipe>("Recipes");

        for (int i = 0; i < CRArray.Length; i++)
        {
            CRList.Add(CRArray[i]);
        }
    }

    void UpdateGraphic()
    {
        if (count < 1)
        {
            item = null;
            itemIcon.gameObject.SetActive(false);
            itemCountText.gameObject.SetActive(false);
        }
        else
        {
            //set sprite to the one from the item
            itemIcon.sprite = item.icon;
            itemIcon.gameObject.SetActive(true);
            itemCountText.gameObject.SetActive(true);
            itemCountText.text = count.ToString();
        }
    }

    public void UseItemInSlot()
    {
        if (CanUseItem())
        {
            item.Use();
            if (item.isConsumable)
            {
                if (count == 1)
                {
                    for (int i = 0; i < craftSlots.Count; i++)
                    {

                        if (craftSlots[i].item == item)
                        {
                            craftSlots[i].ClearCraftSlot();
                        }
                    }
                }
                ApplyEffect();
                Count--;
            }
        }
    }

    private bool CanUseItem()
    {
        return (item != null && count > 0);
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
            UseItemInSlot();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            SendItemToCraft();
            checkForCrafting();
        }
    }

    public void checkForCrafting()
    {
        List<Item> ItemsInCraftList = new List<Item>();
        List<CraftingRecipe> CorrectCountList = new List<CraftingRecipe>();
        int counter = 0;

        for (int i = 0; i < craftSlots.Count; i++)
        {
            if (craftSlots[i].item != null)
            {
                ItemsInCraftList.Add(craftSlots[i].item);
            }
        }

        for (int x = 0; x < CRList.Count; x++)  // for every recipe
        {
            if (CRList[x].Materials.Count == ItemsInCraftList.Count) // check if the count matches the craft slot count
            {
                CorrectCountList.Add(CRList[x]);

                for (int y = 0; y < CorrectCountList.Count; y++) // for every recipe that matches the current count of crafting materials, check if they match
                {
                    for (int z = 0; z < CorrectCountList[y].Materials.Count; z++) // check the materials
                    {
                        if (ItemsInCraftList.Contains(CorrectCountList[y].Materials[z]))
                        {
                            counter += 1;

                            if (CorrectCountList[y].Materials.Count == counter)
                            {
                                // returns the proper recipe 
                                Debug.Log(CorrectCountList[y]);
                                craftResultSlots[0].item = CorrectCountList[y].Results[0];
                                craftResultSlots[0].itemIcon.sprite = CorrectCountList[y].Results[0].icon;
                                craftResultSlots[0].itemIcon.gameObject.SetActive(true);
                                craftResultSlots[0].craftingRecipeVar = CorrectCountList[y];
                            }
                        }
                    }
                    counter = 0;
                }
            }
        }



        ItemsInCraftList.Clear();
    }
    public void SendItemToCraft()
    {
        if (CanUseItem())
        {
            for (int i = 0; i < craftSlots.Count; i++)
            {
                if (craftSlots[i].item == null)
                {
                    craftSlots[i].item = item;
                    craftSlots[i].itemIcon.sprite = item.icon;
                    craftSlots[i].itemIcon.gameObject.SetActive(true);

                    craftResultSlots[0].item = null;
                    craftResultSlots[0].itemIcon.sprite = null;
                    craftResultSlots[0].itemIcon.gameObject.SetActive(false);


                    break;
                }
                else
                {
                    continue;
                }
            }
        }

    }

    public void ApplyEffect()
    {
        switch (item.effectDescription)
        {
            case "STR+":
                CharacterStats[0].ModifyStat(item.effect);
                break;
            case "STR-":
                CharacterStats[0].ModifyStat(item.effect);
                break;
            case "DEX+":
                CharacterStats[1].ModifyStat(item.effect);
                break;
            case "DEX-":
                CharacterStats[1].ModifyStat(item.effect);
                break;
            case "INT+":
                CharacterStats[2].ModifyStat(item.effect);
                break;
            case "INT-":
                CharacterStats[2].ModifyStat(item.effect);
                break;
            case "DEF+":
                CharacterStats[3].ModifyStat(item.effect);
                break;
            case "DEF-":
                CharacterStats[3].ModifyStat(item.effect);
                break;
            case "STA+":
                CharacterStats[4].ModifyStat(item.effect);
                break;
            case "STA-":
                CharacterStats[4].ModifyStat(item.effect);
                break;
            case "HP+":
                CharacterStats[5].ModifyStat(item.effect);
                break;
            case "HP-":
                CharacterStats[5].ModifyStat(item.effect);
                break;
            case "MP+":
                CharacterStats[6].ModifyStat(item.effect);
                break;
            case "MP-":
                CharacterStats[6].ModifyStat(item.effect);
                break;








        }
    }
}
