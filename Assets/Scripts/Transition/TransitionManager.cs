using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    [SceneName] public string startScene;    

    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;

    private bool isFade;
    private bool canTransit;


    private void OnEnable()
    {
        EventHandler.GameStateChangeEvent += OnGameChangeEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameStateChangeEvent -= OnGameChangeEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void OnStartNewGameEvent(int obj)
    {
        StartCoroutine(TransitionToScene("Mene", startScene));
    }

    private void OnGameChangeEvent(GameState gameState)
    {
        canTransit = gameState == GameState.GamePlay;
    }

    //private void Start()
    //{
    //    StartCoroutine(TransitionToScene(string.Empty, startScene));
    //}

    public void Transition(string from, string to)
    {
        if(!isFade && canTransit)
            StartCoroutine(TransitionToScene(from, to));
    }

    private IEnumerator TransitionToScene(string from, string to)
    {
        yield return Fade(1);
        if (from != string.Empty)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return SceneManager.UnloadSceneAsync(from);
        }
        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);

        //设置新的激活场景，假设当前是persistent (index = 0) 和 H1 (index = 1) ，获得新的H1，就是场景总数 - 1
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);

        EventHandler.CallAfterSceneLoadedEvent();
        yield return Fade(0);
    }

    /// <summary>
    /// 场景转换渐变效果，逻辑如下
    /// 1 fade为真，鼠标点击失效
    /// 2 alpha值 0 是透明 ，1 是黑，通过传入想要的透明度与当前透明度的差/时间得到速度
    /// 3 循环如果目标与实际透明度不相等则调用movetowards使其按照速度speed相等
    /// </summary>
    /// <param name="targetAlpha"></param>
    /// <returns></returns>
    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;

        fadeCanvasGroup.blocksRaycasts = true;//场景切换，鼠标无法进行点击

        float speed = Mathf.Abs(targetAlpha - fadeCanvasGroup.alpha) / fadeDuration;

        while(!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        //转换完成，可以点击，不可以再fade
        fadeCanvasGroup.blocksRaycasts = false;

        isFade = false;
    }

    
}
