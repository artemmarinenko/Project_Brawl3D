using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    

    [SerializeField] private Image _background;
    [SerializeField] private Image _thumble;

    [SerializeField] private float _sizeAdjust = 0.9f;

    private Vector3 _startThumplePosition;
    private float _backgroundRadius;

    public void OnPointerDown(PointerEventData eventData)
    {
        _background.transform.position = eventData.position;
        SetBackgroundVisability(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //SetBackgroundVisability(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startThumplePosition = _thumble.transform.position;
        
    }

    private float _distance;
    private Vector3 _newPosition;

    public Vector3 direction { get; private set; }

    public void OnDrag(PointerEventData eventData)
    {

        _distance = Vector3.Distance(_startThumplePosition, eventData.position);
        direction = new Vector3(eventData.position.x, eventData.position.y) - _startThumplePosition;
        direction = direction.normalized;

        GameEvents.RaiseOnEndDragJoystick(direction);

       // _background.transform.position = Vector3.Lerp(_background.transform.position, _newPosition, 2 * Time.deltaTime);
        //_thumble.transform.position = Vector3.Lerp(_thumble.transform.position, _newPosition, 2 * Time.deltaTime);

        if (_distance > _backgroundRadius)
        {
            _newPosition = _startThumplePosition + direction * _backgroundRadius;
            

        }
        else
        {
            direction *= _distance / _backgroundRadius;
            _newPosition = eventData.position;
        }

        _thumble.transform.position = _newPosition;

        
        //Debug.Log($"Direction: {direction}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _thumble.transform.position = _startThumplePosition;
        direction = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
       
        CalculateRadius();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetBackgroundVisability(bool visible)
    {
        _background.gameObject.SetActive(visible);
    }

    private void CalculateRadius()
    {
        var rectTransform = _background.GetComponent<RectTransform>();
        _backgroundRadius = rectTransform.sizeDelta.x * _sizeAdjust * 0.5f;
    }

    


}
