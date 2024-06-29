using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private EffectBase _power;
    
    [SerializeField] private Image _powerImage;
    
    public void Initialize(EffectBase power)
    {
        _power = power;
        _powerImage.sprite = power.EffectIcon; //changes image
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
