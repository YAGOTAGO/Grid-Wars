using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UICharacterSelector : MonoBehaviour
{

    [HideInInspector] public int ID = 0;

    private int _currIndex;
    private bool _insertCharacters = true; //populate the character list the first time around
    private Image _characterImage;
    private Dictionary<int, CardBase> _quantityInitialCards = new();

    [Header("Prefabs")]
    [SerializeField] private CardsPreview _cardPreviewPrefab;

    [Header("Characters")]
    [SerializeField] private List<Character> _characterList = new();
    
    [Header("UI")]
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private VerticalLayoutGroup _cardsDisplayWindow;

    private void Awake()
    {
        _characterImage = GetComponent<Image>();
        _leftButton.onClick.AddListener((LeftButton));
        _rightButton.onClick.AddListener(RightButton);
    }

    private void Start()
    {
        _currIndex = Random.Range(0, _characterList.Count);
        UpdateSelection();
    }

    private void LeftButton()
    {
        _currIndex = (_currIndex - 1 + _characterList.Count) % _characterList.Count;
        UpdateSelection();
    }

    private void RightButton()
    {
        _currIndex = (_currIndex + 1) % _characterList.Count;
        UpdateSelection();   
    }

    private void UpdateSelection()
    {

        Character character = _characterList[_currIndex];

        //Update the selection manager
        if (_insertCharacters)
        {
            CharacterSelection.Instance.SelectedCharacters.Insert(ID, character);
            _insertCharacters = false;
        }
        else
        {
            CharacterSelection.Instance.SelectedCharacters[ID] = character;
        }
        
        //Update the image
        _characterImage.sprite = character.Icon;
    
        //Remove prior cards in preview window
        foreach(Transform child in _cardsDisplayWindow.transform)
        {
            Destroy(child.gameObject);
        }

        //Show the cards of the character in window
        foreach(CardBase card in character.InitialCards)
        {
            CardsPreview preview = Instantiate(_cardPreviewPrefab, _cardsDisplayWindow.transform);
            

        }
    }

}
