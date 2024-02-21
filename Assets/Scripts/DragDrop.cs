using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject objectToDrag;
    public GameObject objectDragToPos;

    public float DropDistance;

    public bool isLocked;

    Vector2 objectInitPos;

    public string StringInToolTip;

    private Animator animator;

    private float timerforanim;
    
    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        objectInitPos = objectToDrag.transform.position;
        timerforanim = 0;
        
    }

    public void DragObject() {
        if(!isLocked) {
            objectToDrag.transform.position = Input.mousePosition;
        }
    }

    public void DropObject() {
        float Distance = Vector3.Distance(objectToDrag.transform.position, objectDragToPos.transform.position);
        if(Distance < DropDistance) {
            isLocked = true;
            objectToDrag.transform.position = objectDragToPos.transform.position;
            animator.SetBool("IsLocked", isLocked);
        }
        else {
            objectToDrag.transform.position= objectInitPos;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        ToolTip.ShowToolTip_Static(StringInToolTip);
    }
    public void OnPointerExit(PointerEventData eventData) {
        ToolTip.HideToolTip_Static();
    }

    public void killGameObj() {
        gameObject.SetActive(false);
    }
    
    /*public void OnPointerDown(PointerEventData eventData) {
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
    }*/
}
