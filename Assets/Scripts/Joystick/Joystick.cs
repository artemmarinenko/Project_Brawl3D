using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public interface IJoystiсk
{
     Vector3 Direction { get; set; }
}

public class Joystick : MonoBehaviour , IJoystiсk, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [SerializeField] protected Image _background;
    [SerializeField] protected Image _thumble;

    [SerializeField] protected float _sizeAdjust = 0.9f;

    protected Vector3 _startThumplePosition;
    protected float _backgroundRadius;
    protected float _distance;
    protected Vector3 _newPosition;

    public Vector3 Direction { get ; set; }

    void Start()
    {
        CalculateRadius();
    }

   
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _background.transform.position = eventData.position;
        SetBackgroundVisability(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       // SetBackgroundVisability(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startThumplePosition = _thumble.transform.position;
        
    }

    

    

    public virtual void OnDrag(PointerEventData eventData)
    {
        ThumbleManipulation(eventData);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _thumble.transform.position = _startThumplePosition;
        Direction = Vector3.zero;
    }

   
   

    protected void SetBackgroundVisability(bool visible)
    {
        _background.gameObject.SetActive(visible);
    }

    protected void CalculateRadius()
    {
        var rectTransform = _background.GetComponent<RectTransform>();
        _backgroundRadius = rectTransform.sizeDelta.x * _sizeAdjust * 0.5f;
    }

    
    protected void ThumbleManipulation(PointerEventData eventData)
    {
        _distance = Vector3.Distance(_startThumplePosition, eventData.position);
        Direction = new Vector3(eventData.position.x, eventData.position.y) - _startThumplePosition;
        Direction = Direction.normalized;



        if (_distance > _backgroundRadius)
        {
            _newPosition = _startThumplePosition + Direction * _backgroundRadius;


        }
        else
        {
            Direction *= _distance / _backgroundRadius;
            _newPosition = eventData.position;
        }

        _thumble.transform.position = _newPosition;

    }

}


