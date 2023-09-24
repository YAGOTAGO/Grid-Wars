using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public static PopUp Instance;
    private GameObject _popUpGO;
    private TextMeshProUGUI _popUpTMP;
    private Coroutine _popUpCoroutine;
    private Image _image;

    private void Awake() // Means must active in scene 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        _image = GetComponent<Image>();
        _popUpGO = gameObject;
        _popUpTMP = GetComponentInChildren<TextMeshProUGUI>();
        Hide(true);
    }

    public void PopUpText(string text)
    {
        if (_popUpCoroutine != null)
        {
            StopCoroutine(_popUpCoroutine);
        }
        
        _popUpCoroutine = StartCoroutine(PopUpCoroutine(text));

    }

    private IEnumerator PopUpCoroutine(string text)
    {
        Hide(false);
        _popUpTMP.text = text;
        yield return new WaitForSeconds(3f);
        Hide(true);
    }

    private void Hide(bool hide) //false: will show | true: will hide
    {
        _image.enabled = !hide;
        _popUpTMP.enabled = !hide;
    }
}
