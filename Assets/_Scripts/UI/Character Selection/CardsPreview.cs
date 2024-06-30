using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CardsPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CardBase _card;
    private GameObject _spawnedCard;
    private Canvas _canvas;
    private float _previewDelay = .3f;

    [SerializeField] private GameObject _cardTemplate;
    [SerializeField] private TextMeshProUGUI _quantityTMP;
    [SerializeField] private TextMeshProUGUI _cardNameTMP;

    public void Initialize(int quantity, CardBase card, Canvas UIcanvas)
    {
        _cardNameTMP.text = card.Name;
        _quantityTMP.text = $"{quantity}x";
        _card = card;
        _canvas = UIcanvas;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        StartCoroutine(PreviewWait());
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        Debug.Log("Exit");
        StopAllCoroutines();
        Destroy(_spawnedCard);
   
    }

    private void ShowCard()
    {

        //Spawn with an offset based on canvas
        RectTransform canvasRectTransform = _canvas.GetComponent<RectTransform>();
        float scale = canvasRectTransform.localScale.x;

        // Offset based on a percentage of the canvas size
        float yOffset = 150 * scale;
        Vector3 offset = new Vector2(0, yOffset);
        Vector3 spawnPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0) + offset;

        //Instatiate the template object
        _spawnedCard = Instantiate(_cardTemplate, spawnPos, Quaternion.identity, _canvas.transform);
        _spawnedCard.transform.localScale = new Vector3(3, 3, 3);

        _spawnedCard.GetComponent<CardDisplay>().Initialize(_card); //Initialize the display
        _spawnedCard.GetComponent<OnCardClick>().enabled = false; //Don't need this added logic, just show card
        _spawnedCard.GetComponent<CardHover>().enabled = false;
    }

    IEnumerator PreviewWait()
    {
        yield return new WaitForSeconds(_previewDelay);
        ShowCard();
    }

}
