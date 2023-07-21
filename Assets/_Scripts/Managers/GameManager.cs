using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState State;
    private bool playerTurn;

    public List<Character> enemyList;
    public List<Character> AllyList;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
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