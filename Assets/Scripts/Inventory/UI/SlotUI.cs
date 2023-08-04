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
        //TODO:仅一个物品做到了点击取消，物品切换未能初始化isSelected状态
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
