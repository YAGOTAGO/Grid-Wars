using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{

    public static CharacterSelection Instance;

    [HideInInspector] public List<Character> SelectedCharacters = new();

    [Header("UI")]
    [SerializeField] private HorizontalLayoutGroup _charactersWindow;
    [SerializeField] private Canvas _canvas;

    [Header("Prefabs")]
    [SerializeField] private UICharacterSelector _ChararacterSelectorPrefab;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        for(int i = 0; i < MapLoader.Instance.NumOfCharacters.Value; i++) 
        {
            //Instantiate and parent it to window
            UICharacterSelector uiCharacterSelector = Instantiate(_ChararacterSelectorPrefab, _charactersWindow.transform);

            //Set the ID number
            uiCharacterSelector.Init(_canvas, i);
        }
    }


}
