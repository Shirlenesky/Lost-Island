using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public RectTransform hand;

    //�����Ļ�õ���Ļ���� -> ת������Ϸ����������
    private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    //�Ƿ��ܵ��
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
            //�����������������Ʒ���Ǵ���ȵȣ�
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
    /// �жϵ����ʲô������������ͬ��Ч��
    /// �߼���
    /// 1 ��������gameobject
    /// 2 ͨ��tag�жϾ�����ʲô
    /// 3 ��ȡ�����������Ľű��������ǰ��Ϊ�գ�ִ�нű�����Ӧ���¼�
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
    /// ������Ϸ�������귵�ص�ǰ�����collider
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