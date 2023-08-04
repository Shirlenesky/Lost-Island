using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameH2A_SO", menuName = "Mini Game Data/GameH2A_SO")]
public class GameH2A_SO : ScriptableObject
{
    [SceneName] public string gameName;

    [Header("球对应的名字和详情")]
    public List<BallDetails> ballDataList;

    [Header("游戏逻辑数据")]
    public List<Connection> connectDataList;

    public List<BallName> startballOrder;

    public BallDetails GetBallDetails(BallName ballName)
    {
        return ballDataList.Find(b => b.ballName == ballName);
    }


}


[System.Serializable]
public class BallDetails
{
    public BallName ballName;

    public Sprite wrongSprite;

    public Sprite rightSprite;
}

[System.Serializable]
public class Connection
{
    public int from;
    public int to;
}