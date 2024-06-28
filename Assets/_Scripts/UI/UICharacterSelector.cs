using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UICharacterSelector : MonoBehaviour
{

    public int ID = 0;

    private int _currIndex;
    private bool _insertCharacters = true;

    [Header("Characters")]
    [SerializeField] private List<Character> _characterList = new();
    
    [Header("UI")]
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    private Image _characterImage;

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

        //Update the selection manager
        if (_insertCharacters)
        {
            CharacterSelection.Instance.SelectedCharacters.Insert(ID, _characterList[_currIndex]);
            _insertCharacters = false;
        }
        else
        {
            CharacterSelection.Instance.SelectedCharacters[ID] = _characterList[_currIndex];
        }
        
        //Update the image
        _characterImage.sprite = _characterList[_currIndex].Icon;
    
        //Show the cards of the character in window

    }

}
