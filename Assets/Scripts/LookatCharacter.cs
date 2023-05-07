using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatCharacter : MonoBehaviour
{   
    private Transform player;
    private Transform closeLookat;
    public InstantiateProjects._Project thisProjectDetails;
    Transform currentParent;
    Transform previousParent;
    bool isRotate = false;

    void Start() 
    {
        player = GameObject.Find("PlayerCapsule").GetComponent<Transform>();
        closeLookat = GameObject.Find("CloseLookAt").GetComponent<Transform>();

        previousParent = transform.parent;
        currentParent = transform.parent;
        //UnityEngine.Debug.Log(thisProjectDetails._projectName);
    }

    void Update()
    {   
        currentParent = transform.parent;

        if (currentParent.gameObject.name == "ProjectHolder") {
            transform.LookAt(closeLookat);
            isRotate = false;
        }

        if (currentParent.gameObject.name == "CubePosition") {
            transform.position = transform.parent.transform.position;
        }

        if (previousParent != currentParent) {     
            if (currentParent.gameObject.name == "CubePosition" && !isRotate) {
                StartCoroutine(LerpRotation(transform.gameObject, transform.rotation, transform.parent.rotation));
            }
        }
        
        if (transform.parent.gameObject.name == "CubePosition" && isRotate)
            transform.rotation = transform.parent.rotation;
        
    }

    IEnumerator LerpRotation (GameObject projBlock, Quaternion startPos, Quaternion endPos) {

        float timeElapsed = 0f;
        float lerpDuration = 0.8f; 
        isRotate = true;

        while (timeElapsed < lerpDuration) {
            projBlock.transform.rotation = Quaternion.Lerp(startPos, endPos, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        projBlock.transform.rotation = endPos;
        //isRotate = false;

    }

    
}
