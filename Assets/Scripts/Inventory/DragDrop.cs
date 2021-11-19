using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Sprite image that gets attached when assigned a game object by the player
    [SerializeField] private Sprite _spriteImage;
    [SerializeField] private Canvas _canvas;


    private readonly float _highlightSize = 1.12f;
    private readonly float _highlightAlpha = .75f;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    [SerializeField] Vector3 _startPos;
    private float _initialScale;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _startPos = _rectTransform.transform.position;
        _initialScale = 1f;
    }


    public Vector3 GetStartPos()
    {
        return _startPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = _highlightAlpha;
        _canvasGroup.blocksRaycasts = false;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * _highlightSize;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        
        transform.localScale = Vector3.one * _initialScale;
    }


    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
         transform.localScale = Vector3.one * _highlightSize;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        transform.localScale = Vector3.one * _initialScale;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
      //  Debug.LogError("pointer down");
    }

}
