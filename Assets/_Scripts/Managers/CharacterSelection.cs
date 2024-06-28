using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{

    public static CharacterSelection Instance;
    public List<Character> SelectedCharacters = new();

    [SerializeField] private UICharacterSelector _prefabSelector;
    [SerializeField] private HorizontalLayoutGroup _window;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        for(int i = 0; i < MapLoader.Instance.NumOfCharacters.Value; i++) 
        {
            //Instantiate and parent it to window
            UICharacterSelector uICharacterSelector = Instantiate(_prefabSelector, _window.transform);

            //Set the ID number
            uICharacterSelector.ID = i;
        }
    }


}
