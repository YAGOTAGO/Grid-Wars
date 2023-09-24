using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private SceneAsset _startScene;
    private Button _playButton;

    private void Awake()
    {
        _playButton = GetComponent<Button>();
        _playButton.onClick.AddListener(() => SwitchScenes());
    }

    private void SwitchScenes()
    {
        SceneManager.LoadScene(_startScene.name);
    }
}
