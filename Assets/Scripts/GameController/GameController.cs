using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, StartMatch, EndGame }
public class GameController : GameData, ISetGameState
{
    [SerializeField] private readonly float delayToStartSpawn = 5;
    public float DelayToStartSpawn => delayToStartSpawn;

    private static GameController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public static GameController GetInstance()
    {
        return instance;
    }

    public GameState GameState { get; private set; } = GameState.MainMenu;

    public void SetGameState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                MainMenu();
                break;
            case GameState.StartMatch:
                StartMatch();
                break;
            case GameState.EndGame:
                EndGame();
                break;
            default:
                throw new ArgumentException($"Invalid game state: {newState}");
        }
    }

    public void MainMenu()
    {
        GameEvent.GetInstance().MainMenu();
        PlayerPrefs.Save();
    }

    public void StartMatch()
    {
        GameEvent.GetInstance().StartMatch();
        StartCoroutine(StartSpawn());
    }

    IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(DelayToStartSpawn);
        GameEvent.GetInstance().StartSpawn();
    }

    public void EndGame()
    {
        GameEvent.GetInstance().EndGame();
        PlayerPrefs.Save();
    }

    /*PENDENCIAS: 
        - CONTROLE DE AUDIO
     */
}
