using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemName ItemName;

    /// <summary>
    /// ���Item�������Item���ӵ�������ͬʱ����������
    /// </summary>
    public void ItemClick()
    {
        //���ӵ�����
        InventoryManager.Instance.AddItem(ItemName);
        //���������
        this.gameObject.SetActive(false);
    }
}