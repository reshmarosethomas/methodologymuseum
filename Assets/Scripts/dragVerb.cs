using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class dragVerb : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{   
    public static GameObject itemBeingDragged;

    Vector3 startPosition;
    Transform startParent;

    #region IBeginDragHandler implementation
    public void OnBeginDrag (PointerEventData eventData) 
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position; 
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    #endregion


    #region IDragHandler implementation
    public void OnDrag(PointerEventData eventData) 
    {   
        transform.position = Input.mousePosition;
    }
    #endregion


    #region IEndDragHandler implementation
    public void OnEndDrag(PointerEventData eventData) 
    {   
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if(transform.parent == startParent) 
        {
            transform.position = startPosition;
        }
    }
    #endregion

}


    //VERSION A
    // public Vector3 startPosition;
    // public Image thisImage;

    // void Start() 
    // {   
    //     thisImage = GetComponent<Image>();
    //     startPosition = transform.position;
    // }

    // public void OnBeginDrag(PointerEventData eventData) 
    // {
    //     thisImage.raycastTarget = false;
    // }

    // public void OnDrag(PointerEventData eventData) 
    // {
    //     transform.position = eventData.position;
    // }

    // public void OnEndDrag(PointerEventData eventData) {
    //     thisImage.raycastTarget = true;
    // }
    
