using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    private bool playerTurn;

    private void Awake()
    {
        Instance = this;
    }
    
    public void UpdateGameState(GameState state)
    {
        State = state;

        switch (state)
        {
            case GameState.PlayerTurn: break;

            default: break;

        }

    }
}

public enum GameState
{
    PlayerTurn,
    EnemyTurn
}