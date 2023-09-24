using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public bool IsWinner = true; //true by default loser swaps scene and sets this to false
    [SerializeField] private SceneAsset _endScene;
    [SerializeField] private GameObject _popUpGO;
    [SerializeField] private TextMeshProUGUI _popUpTMP;
    private Coroutine _popUpCoroutine;
    
    private void Awake()
    {
        Instance = this;
        _popUpGO.SetActive(false);
    }
    
    public void PopUpText(string text)
    {
        if(_popUpCoroutine != null)
        {
            StopCoroutine(_popUpCoroutine);
        }
        
        _popUpCoroutine = StartCoroutine(PopUpCoroutine(text));

    }

    private IEnumerator PopUpCoroutine(string text)
    {
        _popUpGO.SetActive(true);
        _popUpTMP.text = text;
          
        yield return new WaitForSeconds(3f);

        _popUpGO.SetActive(false);
    }
    
    
    [ServerRpc(RequireOwnership = false)]
    public void LoadEndSceneServerRPC()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(_endScene.name, LoadSceneMode.Single);
    }
    
}
