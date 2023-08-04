using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemName ItemName;

    /// <summary>
    /// 点击Item，把这个Item添加到背包，同时场景中隐藏
    /// </summary>
    public void ItemClick()
    {
        //添加到背包
        InventoryManager.Instance.AddItem(ItemName);
        //点击后隐藏
        this.gameObject.SetActive(false);
    }
}
