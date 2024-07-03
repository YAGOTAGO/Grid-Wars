using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CopyToClipboard : MonoBehaviour
{
    private Button _copyButton;
    [SerializeField] private TextMeshProUGUI _copiedTMP;
    [SerializeField] private TextMeshProUGUI _joinCodeTMP;
    
    private void Awake()
    {
        _copyButton = GetComponent<Button>();   
    }

    private void Start()
    {
        _copyButton.onClick.AddListener(Copy);    
    }

    private void Copy()
    {
        StopAllCoroutines();
        GUIUtility.systemCopyBuffer = _joinCodeTMP.text;
        StartCoroutine(Message());
    }

    private IEnumerator Message()
    {
        _copiedTMP.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        _copiedTMP.gameObject.SetActive(false);
    }
}
