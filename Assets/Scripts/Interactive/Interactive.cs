using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public ItemName requireItem;
    public bool isDone;

    public void CheckItem(ItemName itemName)
    {
        if(itemName == requireItem && !isDone)
        {
            isDone = true;
            //ʹ����Ʒ
            OnClickAction();
            EventHandler.CallItemUsedEvent(itemName);
        }
    }


    /// <summary>
    /// Ĭ����ȷ��Ʒִ��
    /// </summary>
    protected virtual void OnClickAction()
    {

    }

    public virtual void EmptyClilck()
    {
        Debug.Log("�յ�");
    }
}