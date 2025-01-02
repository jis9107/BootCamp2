using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]GridLayoutGroup gridLayoutGroup;
    private ItemButton[] buttons;

    private int selectedItemIndex = -1;
    
    // Start is called before the first frame update
    void Awake()
    {
        buttons = gridLayoutGroup.
            GetComponentsInChildren<ItemButton>();

        // ItemManager itemManager = FindObjectOfType<ItemManager>();
        for (var i = 0; i < buttons.Length; i++)
        {
            var i1 = i;
            buttons[i].GetComponent<Button>().
                onClick.AddListener(() => 
                    OnClickItemButton(i1)
                );
        }
    }

    public void AddItem(GettedObject item)
    {
        for (var i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            if (button.itemInfo == null)
            {   
                button.CreateItemInfo(item.ItemData);
                break;
            }
            else if (button.itemInfo.itemData == item.ItemData)
            {
                button.IncreaseAmount();
                break;
            }
        }

        // buttons[i].GetComponent<ItemButton>().ItemInfo = new ItemInfo()
        // {
        //     amount = 1,
        //     itemData = itemData
        // };
    }

    void OnClickItemButton(int index)
    {
        if (0 > selectedItemIndex)
        {
            selectedItemIndex = index;
        }
        else
        {
            var itemButton1 = buttons[selectedItemIndex];
            var itemButton2 = buttons[index];
            
            itemButton1.SwapItem(itemButton2);
            selectedItemIndex = -1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}