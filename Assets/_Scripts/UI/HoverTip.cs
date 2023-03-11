using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string _description = "temp";
    private readonly float _waitTime = 0.5f;
    
    //Interface implementations
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverTipManager.OnMouseLoseFocus();
    }

    //Responisble for showing message
    private void ShowMessage()
    {
        HoverTipManager.OnMouseHover(_description, Input.mousePosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_waitTime);
        ShowMessage();
    }

    public void SetDescription(string descrip)
    {
        _description = descrip;
    }

}
