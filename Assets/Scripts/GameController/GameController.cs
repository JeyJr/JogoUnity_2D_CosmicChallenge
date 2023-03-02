using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, StartMatch, EndGame }
public class GameController : GameData, ISetGameState
{
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
    }

    public void StartMatch()
    {
        GameEvent.GetInstance().StartMatch();
    }

    public void EndGame()
    {
        GameEvent.GetInstance().EndGame();
    }

    /*PENDENCIAS: 
        - CONTROLE DE AUDIO
        - CONTROLE DA DIFICULDADE
     */
}
