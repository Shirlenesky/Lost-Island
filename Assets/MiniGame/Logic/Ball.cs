using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public BallDetails ballDetails;

    public bool isMatch;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetupBall(BallDetails ball)
    {
        ballDetails = ball;

        if(isMatch)
        {
            SetRight();
        }
        else
        {
            SetWrong();
        }
    }

    /// <summary>
    /// 这两个set不是指当前具体是哪个球的图，而是我移动了这个球，固定确定这个球移到了这个holder，应该使用他的蓝图还是灰土的一个设置判断
    /// </summary>
    public void SetRight()
    {
        spriteRenderer.sprite = ballDetails.rightSprite;
    }
    public void SetWrong()
    {
        spriteRenderer.sprite = ballDetails.wrongSprite;
    }
}
