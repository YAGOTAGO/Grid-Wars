using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GoToSceneButton : MonoBehaviour
{
    [SerializeField] private string _destinationScene;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => SwitchScenes());
    }

    private void SwitchScenes()
    {
        SceneManager.LoadScene(_destinationScene);
    }
}
