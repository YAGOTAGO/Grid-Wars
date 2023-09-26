using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectTipWindow : MonoBehaviour
{
    public static EffectTipWindow Instance;

    #region TipWindow
    [Header("Tip window settings")]
    [SerializeField] private GameObject _tipWindowPrefab;
    [SerializeField] private int MaxWidth = 150;

    [Header("References to UI")]
    [SerializeField] private Canvas _UICanvas;
    [SerializeField] private GameObject _UIPlayerHorizontalGroup;

    private TextMeshProUGUI _tipText;
    private RectTransform _tipWindow;
    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;
    #endregion

    private void Start()
    {
        Instance = this;
        InitTipPrefab();
        HideTip();
    }

    private void InitTipPrefab()
    {
        _tipWindowPrefab = Instantiate(_tipWindowPrefab);
        _tipWindowPrefab.transform.SetParent(_UICanvas.transform);
        _tipText = _tipWindowPrefab.GetComponentInChildren<TextMeshProUGUI>();
        _tipWindow = _tipWindowPrefab.GetComponent<RectTransform>();
        _tipWindow.localScale = Vector3.one;
    }

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    public void SetPlayerUI(GameObject playerUI)
    {
        playerUI.transform.SetParent(_UIPlayerHorizontalGroup.transform);
    }

    //Show the tipwindow with the size set up
    private void ShowTip(string tip, Vector2 mousePos)
    {
        _tipWindow.gameObject.SetActive(true);
        _tipText.text = tip;
        _tipWindow.sizeDelta = new Vector2(_tipText.preferredWidth > MaxWidth ? MaxWidth : _tipText.preferredWidth, _tipText.preferredHeight);
        _tipWindow.transform.position = new Vector2(mousePos.x + (_tipWindow.sizeDelta.x/2), mousePos.y);
 
    }

    public void HideTip()
    {
        _tipWindow.gameObject.SetActive(false);
    }

}
