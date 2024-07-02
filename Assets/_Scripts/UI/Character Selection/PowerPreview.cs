using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerPreview : HoverTip
{
   
    [SerializeField] private Image _powerImage;
    
    public void UpdatePowerPreview(EffectBase power)
    {
        _powerImage.sprite = power.EffectIcon; //changes image
    }

}
