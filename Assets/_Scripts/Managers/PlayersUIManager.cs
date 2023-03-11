using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayersUIManager : MonoBehaviour
{
    public static PlayersUIManager Instance;

    #region TipWindow
    public TextMeshProUGUI TipText;
    public RectTransform TipWindow;
    public int MaxWidth = 150;
    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;
    #endregion

    #region Players UI
    [SerializeField] private GameObject _UIPlayerHorizontalGroup;
    #endregion


    private void Start()
    {
        Instance = this;
        HideTip();
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

    private void ShowTip(string tip, Vector2 mousePos)
    {
        TipText.text = tip;
        TipWindow.sizeDelta = new Vector2(TipText.preferredWidth > MaxWidth ? MaxWidth : TipText.preferredWidth, TipText.preferredHeight);
        
        TipWindow.transform.position = new Vector2(mousePos.x + (TipWindow.sizeDelta.x/2), mousePos.y);
        TipWindow.gameObject.SetActive(true);
        
    }

    private void HideTip()
    {
        //TipText = default;
        TipWindow.gameObject.SetActive(false);
    }

}
