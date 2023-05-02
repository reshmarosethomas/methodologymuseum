using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class snapToSlot : MonoBehaviour, IDropHandler
{   
    public GameObject slotNext;
    public int slotNum;
    

    public GameObject item {
        get {
            if (transform.childCount > 0) {
                return transform.GetChild(0).gameObject;
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

            activateNextSlot(true);
        }
    }
    #endregion

    private void Update() {
        if (transform.childCount <= 0) {
            activateNextSlot(false);
        }
    }

    private void activateNextSlot(bool filled) {
        if (slotNext!=null)
        {   
            if (filled) {
            slotNext.SetActive(true);
            }
            else {
                if (slotNext.transform.childCount <=0) 
                {
                    slotNext.SetActive(false);
                }
            }
        }
    }
}
