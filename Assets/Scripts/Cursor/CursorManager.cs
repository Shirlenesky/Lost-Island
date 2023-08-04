using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public RectTransform hand;

    //点击屏幕得到屏幕坐标 -> 转换成游戏中世界坐标
    private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    //是否能点击
    private bool canClick;
    private bool holdItem;

    private ItemName currItemName;



    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
    }
    
    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
    }
    

    private void Update()
    {
        canClick = ObjectAtMousePosition();
        
        if(hand.gameObject.activeInHierarchy)
        {
            hand.position = Input.mousePosition;
        }

        if (InteractWithUI()) return;

        if(canClick && Input.GetMouseButtonDown(0))
        {
            //检测点击情况（点击是物品还是传输等等）
            ClickAction(ObjectAtMousePosition().gameObject);
        }
    }

    private void OnItemUsedEvent(ItemName itemName)
    {
        currItemName = ItemName.None;
        holdItem = false;
        hand.gameObject.SetActive(false);
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        holdItem = isSelected;
        
        if(isSelected)
        {
            Debug.Log("*");
            currItemName = itemDetails.itemName;
        }
        hand.gameObject.SetActive(holdItem);

    }

    

    /// <summary>
    /// 判断点击是什么东西来发出不同的效果
    /// 逻辑：
    /// 1 传入点击的gameobject
    /// 2 通过tag判断具体是什么
    /// 3 获取触发器包含的脚本，如果当前不为空，执行脚本中相应的事件
    /// </summary>
    /// <param name="clickObject"></param>
    private void ClickAction(GameObject clickObject)
    {
        switch(clickObject.tag)
        {
            case "Teleport":
                var teleport = clickObject.GetComponent<Teleport>();
                teleport?.TeleportToScene();
                break;
            case "Item":
                var item = clickObject.GetComponent<Item>();
                item?.ItemClick();
                break;
            case "Interactive":
                var interactive = clickObject.GetComponent<Interactive>();
                if(holdItem)
                {
                    interactive?.CheckItem(currItemName);
                }
                else
                {
                    interactive?.EmptyClilck();
                }
                break;
        }
    }

    /// <summary>
    /// 传入游戏世界坐标返回当前点击的collider
    /// </summary>
    /// <returns></returns>
    private Collider2D ObjectAtMousePosition()
    {
        return Physics2D.OverlapPoint(mouseWorldPos);
    }

    private bool InteractWithUI()
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
}
