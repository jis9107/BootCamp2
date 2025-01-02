using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwanedItem : MonoBehaviour
{
    // Action은 리턴 값이 없는 함수를 담을 때 사용한다.
    public Action OnDestroiedAction;
    public ItemData itemData;

    public void SetItemData(ItemData itemData)
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        this.itemData = itemData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        // OnDestroiedAction null이라면 . 뒤에부터는 실행하지 않는다.
        OnDestroiedAction?.Invoke();
    }
}