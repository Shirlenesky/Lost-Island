using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : Interactive
{
    public BallName matchBall;

    private Ball currectBall;

    public HashSet<Holder> linkHolders = new HashSet<Holder>();

    public bool isEmpty;

    public void CheckBall(Ball ball)
    {
        currectBall = ball;
        if(currectBall.ballDetails.ballName == matchBall)
        {
            currectBall.isMatch = true;
            currectBall.SetRight();
        }
        else
        {
            currectBall.isMatch = false;
            currectBall.SetWrong();
        }
    }
    public override void EmptyClilck()
    {
        foreach (var holder in linkHolders)
        {
            if(holder.isEmpty)
            {
                //移动球
                currectBall.transform.position = holder.transform.position;
                currectBall.transform.SetParent(holder.transform);
                //交换球
                holder.CheckBall(currectBall);
                this.currectBall = null;

                //状态改变
                this.isEmpty = true;
                holder.isEmpty = false;

                EventHandler.CallCheckGameStateEvent();
            }
        }
    }
}
