using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : Singleton<GameController>
{
    public UnityEvent OnFinish;


    [Header("ÓÎÏ·Êý¾Ý")]
    public GameH2A_SO gameData;
    public GameH2A_SO[] gameDataArray;

    public GameObject LineParent;

    public LineRenderer LinePrefab;

    public Ball ballPrefab;

    public Transform[] holderTransform;

    private void OnEnable()
    {
        EventHandler.CheckGameStateEvent += OnCheckGameStateEvent;
    }
    private void OnDisable()
    {
        EventHandler.CheckGameStateEvent -= OnCheckGameStateEvent;
    }

    private void OnCheckGameStateEvent()
    {
        foreach (var ball in FindObjectsOfType<Ball>())
        {
            if (!ball.isMatch) return;
        }

        foreach (var holder in holderTransform)
        {
            holder.GetComponent<Collider2D>().enabled = false;
        }
        
        EventHandler.CallGamePassEvent(gameData.gameName);
        
        OnFinish?.Invoke();
    }

    private void Start()
    {
        DrawBall();
        DrawLine();
    }

    public void ResetGame()
    {
        foreach (var holder in holderTransform)
        {
            if(holder.childCount > 0)
            {
                Destroy(holder.GetChild(0).gameObject);
            }
        }
        DrawBall();
    }

    public void DrawLine()
    {
        foreach (var connect in gameData.connectDataList)
        {
            var line = Instantiate(LinePrefab, LineParent.transform);

            line.SetPosition(0, holderTransform[connect.from].position);
            line.SetPosition(1, holderTransform[connect.to].position);

            holderTransform[connect.from].GetComponent<Holder>().linkHolders.Add(holderTransform[connect.to].GetComponent<Holder>());
            holderTransform[connect.to].GetComponent<Holder>().linkHolders.Add(holderTransform[connect.from].GetComponent<Holder>());
        }
    }

    public void DrawBall()
    {
        for (int i = 0; i < gameData.startballOrder.Count; i++)
        {
            if(gameData.startballOrder[i] == BallName.None)
            {
                holderTransform[i].GetComponent<Holder>().isEmpty = true;
                continue;
            }

            Ball ball = Instantiate(ballPrefab, holderTransform[i]);

            holderTransform[i].GetComponent<Holder>().CheckBall(ball);
            holderTransform[i].GetComponent<Holder>().isEmpty = false;
            ball.SetupBall(gameData.GetBallDetails(gameData.startballOrder[i]));
        }
    }

    public void SetWeekGameData(int week)
    {
        gameData = gameDataArray[week];
        DrawLine();
        DrawBall();
    }

    
}
