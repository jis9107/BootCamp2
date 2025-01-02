using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public ItemInfo itemInfo;
    
    [SerializeField] private Image itemImage;
    [SerializeField] TMP_Text itemCountText;
    void SetItemImage(Sprite sprite)
    {
        itemImage.sprite = sprite;
        if (sprite == null)
        {
            var color = itemImage.color;
            color.a = 0;
            itemImage.color = color;
        }
        else
        {
            var color = itemImage.color;
            color.a = 1f;
            itemImage.color = color;
        }
    }

    public void CreateItemInfo(ItemData _itemData)
    {
        itemInfo = new ItemInfo()
        {
            itemData = _itemData,
            amount = 1,
        };
        VisualUpdate();
    }

    public void IncreaseAmount()
    {
        itemInfo.amount += 1;
        VisualUpdate();
    }


    public void VisualUpdate()
    {
        SetItemImage(itemInfo.itemData.icon);
        itemCountText.text = itemInfo.amount.ToString();
    }

    public void SwapItem(ItemButton target)
    {
        (itemInfo, target.itemInfo) = (target.itemInfo, itemInfo);
        VisualUpdate();
        target.VisualUpdate();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
