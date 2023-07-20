using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _cardHandGroup;
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

    private void Start()
    {
        CardDisplayManager.Instance.InstantiateCard(CardDatabase.Instance.GetCardByName(CardName.card2), _cardHandGroup.transform);
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