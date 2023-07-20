using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float _vertMove = 30f;
    [SerializeField] private float _moveTime = 0.1f;
    [Range(0f, 2f), SerializeField] private float _scaleAmount = 1.1f;

    private Vector3 _startPos;
    private Vector3 _startScale;

    void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
        
    }

    private IEnumerator MoveCard(bool startigAnim)
    {
        Vector3 endPosition;
        Vector3 endScale;

        float elapsedTime = 0f;

        while(elapsedTime< _moveTime)
        {
            elapsedTime += Time.deltaTime;

            if(startigAnim )
            {
                endPosition = _startPos + new Vector3(0f, _vertMove, 0f);
                endScale = _startScale * _scaleAmount;
            }
            else
            {
                endPosition = _startPos;
                endScale = _startScale;
            }

            //Calculate Lerp
            Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / _moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / _moveTime));
            
            //Apply the changes
            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(MoveCard(true));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(MoveCard(false));
    }
}
