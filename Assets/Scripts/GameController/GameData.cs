using System;
using UnityEngine;


public class GameData : MonoBehaviour
{
    [Header("GameMode")]
    [SerializeField] private int gameModeValue = 4;
    [SerializeField] private string[] gameModeName = new[]
    {
        "Muito Fácil",
        "Fácil",
        "Normal",
        "Humano",
        "Difícil",
        "Muito Difícil",
        "Insano",
        "Alienígena",
        "Astronômico",
        "Deus"
    };

    [Header("BoxControl")]
    [SerializeField] private int collectedItems = 0;
    [SerializeField] private int maxItems = 1;
    public int CollectedItems => collectedItems;
    public int MaxItems => maxItems;

    [Header("TimeControl")]
    [SerializeField] private TimeSpan recordTime;
    [SerializeField] private TimeSpan durationOfMatch;
    public TimeSpan RecordTime => recordTime;

    private readonly string recordKey = "record";
    private readonly string gameModeKey = "gameMode";



    private void Start()
    {
        CheckSavedRecord();
        CheckSaveGameMode();
        SetItemsEvent();
    }

    #region Box: AddCollectedItems, AddCollectedItems, RemoveCollectedItems

    private void SetItemsEvent()
    {
        GameEvent.GetInstance().OnStartMatch += ResetCollectedItems;
        GameEvent.GetInstance().OnCollectItems += AddCollectedItems;
        GameEvent.GetInstance().OnRemoveItems += RemoveCollectedItems;
    }

    private void AddCollectedItems()
    {
        collectedItems++;

        if(collectedItems >= maxItems)
        {
            GameController.GetInstance().SetGameState(GameState.EndGame);
        }
    }

    private void RemoveCollectedItems()
    {
        if(collectedItems > 0)
        {
            collectedItems--;
        }
    }

    private void ResetCollectedItems()
    {
        collectedItems = 0;
    }
    #endregion

    #region Record: CheckSavedRecord, CheckNewRecord, SetDurationMatch, GetDurationMatch, FormatTimeSpan
    private void CheckSavedRecord()
    {
        if (!PlayerPrefs.HasKey(recordKey))
        {
            PlayerPrefs.SetString(recordKey, TimeSpan.Zero.ToString());
        }

        recordTime = TimeSpan.Parse(PlayerPrefs.GetString(recordKey));
    }

    /// <summary>
    /// Verifica e salva o novo recorde
    /// </summary>
    /// <returns>Retorna o valor atual do recorde formatado mm:ss </returns>
    public string CheckNewRecord()
    {
        if(collectedItems >= maxItems)
        {
            if (recordTime <= TimeSpan.Zero || durationOfMatch < recordTime)
            {
                //new Recorde
                PlayerPrefs.SetString(recordKey, durationOfMatch.ToString());
                return "Novo recorde: " + FormatTimeSpan(durationOfMatch);
            }
        }

        return "Recorde: " + FormatTimeSpan(recordTime);
    }


    public void SetDurationMatch(TimeSpan duration)
    {
        durationOfMatch = duration;
    }

    public string GetDurationMatch()
    {
        return FormatTimeSpan(durationOfMatch);
    }

    private string FormatTimeSpan(TimeSpan time)
    {
        return string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
    }

    #endregion

    #region GameMode: CheckSaveGameMode, AddGameModeValue, SubGameModeValue, GetGameModeValue, GetGameModeName, GetGameModeLength
    

    private void CheckSaveGameMode()
    {
        if (!PlayerPrefs.HasKey(gameModeKey))
        {
            PlayerPrefs.SetInt(gameModeKey, 4);
        }

        gameModeValue = PlayerPrefs.GetInt(gameModeKey);
    }

    public void AddGameModeValue()
    {
        if(gameModeValue < gameModeName.Length - 1)
        {
            gameModeValue++;
        }
        PlayerPrefs.SetInt(gameModeKey, gameModeValue);
    }

    public void SubGameModeValue()
    {
        if(gameModeValue > 0)
        {
            gameModeValue--;
        }

        PlayerPrefs.SetInt(gameModeKey, gameModeValue);
    }

    public int GetGameModeValue()
    {
         return gameModeValue;
    }

    public string GetGameModeName()
    {
        return gameModeName[gameModeValue];
    }

    public int GetGameModeLength()
    {
        return gameModeName.Length;
    }
    #endregion

    #region Setup: GetMoveSpeed, GetSpawnSpeed
    public float GetMoveSpeed(float currentValue)
    {
        return currentValue + gameModeValue;
    }

    public float GetSpawnSpeed(float currentValue) 
    {
        return currentValue - (gameModeValue * .2f);
    }
    #endregion

}
