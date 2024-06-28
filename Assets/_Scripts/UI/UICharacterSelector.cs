using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UICharacterSelector : MonoBehaviour
{

    private int _currIndex;

    [Header("Characters")]
    [SerializeField] private List<Character> _characterList = new();
    
    [Header("UI")]
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    private Image _characterImage;

    private void Awake()
    {
        _characterImage = GetComponent<Image>();
        _leftButton.onClick.AddListener(LeftButton);
        _rightButton.onClick.AddListener(RightButton);
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

    private void Start()
    {
        _currIndex = Random.Range(0, _characterList.Count);
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        _characterImage.sprite = _characterList[_currIndex].Icon;
    }

}
