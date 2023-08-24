using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconCharacterHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Character _character;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _character.HighlightCharacter(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _character.HighlightCharacter(false);
    }
}
