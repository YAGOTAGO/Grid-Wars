using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UICharacterSelector : MonoBehaviour
{

    private int ID = 0;
    private int _currIndex;
    private bool _insertCharacters = true; //populate the character list the first time around
    private Image _characterImage;
    private Canvas _canvas;
    private Dictionary<Character, Dictionary<CardBase, int>> _characterToQuantities = new();
 
    [Header("Prefabs")]
    [SerializeField] private CardsPreview _cardPreviewPrefab;

    [Header("Characters")]
    [SerializeField] private List<Character> _characterList = new();
    
    [Header("UI")]
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private VerticalLayoutGroup _cardsDisplayWindow;
    [SerializeField] private PowerPreview _powerPreview;

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

    public void Init(Canvas canvas, int ID)
    {
        _canvas = canvas;
        this.ID = ID;
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
        //New character selected
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

        //Update power preview
        _powerPreview.Initialize(character.StartingEffect);

        //Update the image
        _characterImage.sprite = character.Icon;
    
        //Remove prior cards in preview window
        foreach(Transform child in _cardsDisplayWindow.transform)
        {
            Destroy(child.gameObject);
        }

        //Count cards
        CardQuantities(character);

        Dictionary<CardBase, int> quantities = _characterToQuantities[character];

        foreach(KeyValuePair<CardBase, int> entry in quantities)
        {
            CardBase card = entry.Key;
            CardsPreview preview = Instantiate(_cardPreviewPrefab, _cardsDisplayWindow.transform);
            preview.Initialize(entry.Value, card, _canvas);
        }
    }

    private void CardQuantities(Character character)
    {

        if (!_characterToQuantities.ContainsKey(character)) //we havent counted the cards for that character
        {
            Dictionary<CardBase, int> cardsToQuantities = new();
            foreach (CardBase card in character.InitialCards)
            {
                if (cardsToQuantities.ContainsKey(card))
                {
                    cardsToQuantities[card]++; //Count
                }
                else
                {
                    cardsToQuantities[card] = 1;
                }
                
            }
            _characterToQuantities[character] = cardsToQuantities; //Update the character dict
        }

    }
}
