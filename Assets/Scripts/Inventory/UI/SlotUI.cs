using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler ,IPointerExitHandler
{
    public Image itemImage;

    public ItemTooltip itemTooltip;

    private ItemDetails currItem;

    private bool isSelected;

    public void SetItem(ItemDetails itemDetails)
    {
        currItem = itemDetails;
        this.gameObject.SetActive(true);
        itemImage.sprite = itemDetails.itemSprite;
        itemImage.SetNativeSize();
    }
    
    public void SetEmpty()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO:��һ����Ʒ�����˵��ȡ������Ʒ�л�δ�ܳ�ʼ��isSelected״̬
        isSelected = !isSelected;
        EventHandler.CallItemSelectedEvent(currItem, isSelected);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.gameObject.activeInHierarchy)
        {
            itemTooltip.gameObject.SetActive(true);
            itemTooltip.UpdateItemName(currItem.itemName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemTooltip.gameObject.SetActive(false);
    }
    
}