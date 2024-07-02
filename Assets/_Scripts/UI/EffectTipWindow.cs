using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EffectTipWindow : Singleton<EffectTipWindow>
{
    #region TipWindow
    [Header("Tip window settings")]
    [SerializeField] private GameObject _tipWindowPrefab;
    private readonly int _maxWidth = 220;

    [Header("References to UI")]
    [SerializeField] private Canvas _UICanvas;

    private TextMeshProUGUI _tipText;
    private RectTransform _tipWindow;
    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;
    #endregion

    protected override void Awake()
    {
        base.Awake();
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

    //Show the tipwindow with the size set up
    private void ShowTip(string tip, Vector2 mousePos)
    {
        _tipWindow.gameObject.SetActive(true);
        _tipText.text = tip;

        //Change size delta twice because width affects the preffered height
        _tipWindow.sizeDelta = new Vector2(_tipText.preferredWidth > _maxWidth ? _maxWidth : _tipText.preferredWidth, _tipWindow.sizeDelta.y);
        _tipWindow.sizeDelta = new Vector2(_tipWindow.sizeDelta.x, _tipText.preferredHeight);

        OffsetTip(mousePos);
    }

    public void HideTip()
    {
        _tipWindow.gameObject.SetActive(false);
    }

    private void OffsetTip(Vector2 mousePos)
    {
        RectTransform canvasRectTransform = _UICanvas.GetComponent<RectTransform>();
        float scale = canvasRectTransform.localScale.x;

        float xOffset = 150 * scale; //offset based on canvas scale so is always same

        _tipWindow.transform.position = new Vector2(mousePos.x + xOffset, mousePos.y);
    }
}
