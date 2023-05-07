using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class disablePhyRaycast : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{   
    public bool disable3DRaycast = false;

    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData){   
        //UnityEngine.Debug.Log($"The cursor entered the {this.gameObject.name} element.");
        disable3DRaycast = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        //UnityEngine.Debug.Log($"The cursor exited the {this.gameObject.name} element.");
        disable3DRaycast = false;
    }
}
