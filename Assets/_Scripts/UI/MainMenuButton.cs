using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainMenuButton : NetworkBehaviour
{
    private Button _button;
    [SerializeField] private SceneAsset _sceneToLoad;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => GoToMainMenu());
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += Disconnect;
    }

    private void Disconnect(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (IsServer)
        {
            foreach (ulong client in clientsCompleted)
            {
                if(OwnerClientId != client)
                {
                    NetworkManager.DisconnectClient(client);
                }
            }
            NetworkManager.Shutdown();
        }

    }
    private void GoToMainMenu()
    {
        
        //Load the main menu
        SceneManager.LoadScene(_sceneToLoad.name, LoadSceneMode.Single);
    }

   
}
