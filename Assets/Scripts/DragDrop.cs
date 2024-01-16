using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public void OnPointerDown(PointerEventData eventData) {
        UnityEngine.Debug.Log("OnPointerDown");
    }
    
    public void OnBeginDrag(PointerEventData eventData) {
        UnityEngine.Debug.Log("OnBeginDrag");
    }
    public void OnEndDrag(PointerEventData eventData) {
        UnityEngine.Debug.Log("OnEndDrag");
    }
    public void OnDrag(PointerEventData eventData) {
        UnityEngine.Debug.Log("OnDrag");
    }
}
