using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IconCharacterHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Character _character;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    public void SetCharacter(Character character)
    {
        _character = character;
        _image.sprite = character.Icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _character.HighlightCharacter(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _character.HighlightCharacter(false);
    }
}
