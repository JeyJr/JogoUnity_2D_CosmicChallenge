using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour 
{
    private static GameEvent instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static GameEvent GetInstance()
    {
        return instance;
    }

    public event Action OnMainMenu;
    public event Action OnStartMatch;
    public event Action OnEndGame;

    public event Action OnCollectItems;
    public event Action OnRemoveItems;

    public void StartMatch()
    {
        OnStartMatch?.Invoke();
    }

    public void MainMenu()
    {
        OnMainMenu?.Invoke();
    }

    public void EndGame()
    {
        OnEndGame?.Invoke();
    }

    public void CollectItems()
    {
        OnCollectItems?.Invoke();
    }

    public void RemoveItems()
    {
        OnRemoveItems?.Invoke();
    }
}

