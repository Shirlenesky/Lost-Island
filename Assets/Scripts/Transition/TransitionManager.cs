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

        //�����µļ���������赱ǰ��persistent (index = 0) �� H1 (index = 1) ������µ�H1�����ǳ������� - 1
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);

        EventHandler.CallAfterSceneLoadedEvent();
        yield return Fade(0);
    }

    /// <summary>
    /// ����ת������Ч�����߼�����
    /// 1 fadeΪ�棬�����ʧЧ
    /// 2 alphaֵ 0 ��͸�� ��1 �Ǻڣ�ͨ��������Ҫ��͸�����뵱ǰ͸���ȵĲ�/ʱ��õ��ٶ�
    /// 3 ѭ�����Ŀ����ʵ��͸���Ȳ���������movetowardsʹ�䰴���ٶ�speed���
    /// </summary>
    /// <param name="targetAlpha"></param>
    /// <returns></returns>
    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;

        fadeCanvasGroup.blocksRaycasts = true;//�����л�������޷����е��

        float speed = Mathf.Abs(targetAlpha - fadeCanvasGroup.alpha) / fadeDuration;

        while(!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        //ת����ɣ����Ե������������fade
        fadeCanvasGroup.blocksRaycasts = false;

        isFade = false;
    }

    
}