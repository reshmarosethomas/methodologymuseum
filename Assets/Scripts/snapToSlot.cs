using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class snapToSlot : MonoBehaviour, IDropHandler
{   
    public GameObject slotNext;
    public int slotNum;
    public bool isFilled = false;
    GameObject placeHolderText;

    public GameObject item {
        get {
            if (transform.childCount > 1) {
                return transform.GetChild(1).gameObject;
            }
            return null;
        }
    }

    #region IDropHandler implementation
    public void OnDrop (PointerEventData eventData) 
    {
        if (!item) 
        {
            dragVerb.itemBeingDragged.transform.SetParent (transform);
            isFilled = true;
            
        }
    }
    #endregion

    private void Start() {
        placeHolderText = transform.GetChild(0).gameObject;
        UnityEngine.Debug.Log(placeHolderText.name);
    }

    private void Update() {
        if (transform.childCount <= 1) {

            isFilled = false;

            if (transform.parent.gameObject.name=="BottomPanel" && !transform.GetChild(0).gameObject.activeSelf)
                transform.GetChild(0).gameObject.SetActive(true);

        } else if (transform.childCount > 1) {

            if (transform.parent.gameObject.name=="BottomPanel" && transform.GetChild(0).gameObject.activeSelf)
                transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // private void activateNextSlot(bool filled) {
    //     if (slotNext!=null)
    //     {   
    //         if (filled) {
    //         slotNext.SetActive(true);
    //         }
    //         else {
    //             if (slotNext.transform.childCount <=0) 
    //             {
    //                 slotNext.SetActive(false);
    //             }
    //         }
    //     }
    // }
}
