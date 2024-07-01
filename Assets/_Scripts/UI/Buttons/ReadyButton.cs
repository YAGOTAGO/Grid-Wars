using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReadyButton : NetworkBehaviour
{
    
    private Button _readyButton;
    [SerializeField] private Image _image;

    private void Awake()
    {
        _readyButton = GetComponent<Button>();
    }

    public override void OnNetworkSpawn()
    {
        _readyButton.onClick.AddListener(ReadyButtonClicked);

    }

    private void ReadyButtonClicked()
    {
        _image.gameObject.SetActive(true);

        List<Character> selectedCharacters = CharacterSelection.Instance.SelectedCharacters;

        if (IsServer)
        {
            CharacterSpawner.Instance.ServerCharacters = selectedCharacters;
            if(CharacterSpawner.Instance.ClientCharacters.Count> 0) //If we have both character information then load the next scene
            {
                NetworkManager.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            }

        }
        else
        {
            string[] stringArr = new string[selectedCharacters.Count];
            for (int i = 0; i < selectedCharacters.Count; i++)
            {
                stringArr[i] = selectedCharacters[i].name;
            }

            //Send a serverRPC to set characters
            Strings charactersAsString = new(stringArr);

            SetClientCharactersServerRpc(charactersAsString);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetClientCharactersServerRpc(Strings characters)
    {
        for(int i = 0; i<characters.MyStrings.Length; i++)
        {
            if (characters.MyStrings[i] != null)
            {
                CharacterSpawner.Instance.ClientCharacters.Add(Database.Instance.GetCharacterByName(characters.MyStrings[i]));
            }
        }

        if (CharacterSpawner.Instance.ServerCharacters.Count > 0) //If we have both character information then load the next scene
        {
            NetworkManager.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }

}
