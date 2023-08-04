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
            //使用物品
            OnClickAction();
            EventHandler.CallItemUsedEvent(itemName);
        }
    }


    /// <summary>
    /// 默认正确物品执行
    /// </summary>
    protected virtual void OnClickAction()
    {

    }

    public virtual void EmptyClilck()
    {
        Debug.Log("空点");
    }
}
