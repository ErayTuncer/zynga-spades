using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HolderElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

    public UnityEvent OnDragging = new UnityEvent();
    public UnityEvent OnDragEnd = new UnityEvent();

    private bool isDragged = false;

    public void OnPointerDown(PointerEventData eventData) {
        isDragged = true;
    }

    public void OnDrag(PointerEventData eventData) {
        Vector3 nextPosition = transform.position;
        nextPosition.x = eventData.position.x;
        nextPosition.y = eventData.position.y;
        transform.position = nextPosition;

        OnDragging.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData) {
        isDragged = false;
        OnDragEnd.Invoke();
    }

    public bool IsDragged() {
        return isDragged;
    }

}