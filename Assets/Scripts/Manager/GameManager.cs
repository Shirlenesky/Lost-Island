using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, bool> miniGameStateDict = new Dictionary<string, bool>();

    private GameController currentGame;
    private int gameWeek;

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.GamePassEvent += OnGamePassEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.GamePassEvent -= OnGamePassEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void OnStartNewGameEvent(int gameWeek)
    {
        this.gameWeek = gameWeek;
        miniGameStateDict.Clear();
    }

    void Awake()
    {
        SceneManager.LoadScene("Mene", LoadSceneMode.Additive);
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
    }

    private void OnAfterSceneLoadedEvent()
    {
        foreach (var minigame in FindObjectsOfType<MiniGame>())
        {
            if (miniGameStateDict.TryGetValue(minigame.gameName, out bool isPass))
            {
                minigame.isPass = isPass;
                minigame.UpdateMiniGameState();
            }
        }

        currentGame = FindObjectOfType<GameController>();
        currentGame?.SetWeekGameData(gameWeek);
    }

    private void OnGamePassEvent(string gameName)
    {
        miniGameStateDict[gameName] = true;
    }
}
