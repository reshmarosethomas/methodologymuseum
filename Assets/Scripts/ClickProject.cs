using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickProject : MonoBehaviour
{   
    public bool enableRay = false;

    Transform[] verbSlots = new Transform[5];
    Transform[] bottomSlots = new Transform[4];

    GameObject bottomPanel;
    GameObject verbPanel;
    GameObject clearButton;
    GameObject baseOverlay;

    selectProject _selectProject;

    // Start is called before the first frame update
    void Start()
    {
        bottomPanel = GameObject.Find("Canvas/BottomPanel");
        verbPanel = GameObject.Find("Canvas/VerbPanel");
        clearButton = GameObject.Find("Canvas/ClearButton");
        baseOverlay = GameObject.Find("Canvas/Base");

        _selectProject = GetComponent<selectProject>();

        for (int i = 0; i<5; i++) {
            verbSlots[i] = verbPanel.transform.GetChild(i).GetComponent<Transform>();
        }

        for (int i = 0; i<4; i++) {
            bottomSlots[i] = bottomPanel.transform.GetChild(i).GetComponent<Transform>();
        }

        enableRay = true;
        //baseOverlay.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {   
        if (!bottomPanel.GetComponent<disablePhyRaycast>().disable3DRaycast && !verbPanel.GetComponent<disablePhyRaycast>().disable3DRaycast) {
            if (clearButton.activeSelf) {
                if (!clearButton.GetComponent<disablePhyRaycast>().disable3DRaycast) {
                    enableRay = true;
                }
                else {
                    enableRay = false;
                }
            } else {
                enableRay = true;
            }
        } else {
            enableRay = false;
        }

        //UnityEngine.Debug.Log(enableRay);

        if (Input.GetMouseButtonDown(0)) {
            if (!this.GetComponent<selectProject>().projInFocus && enableRay) {
                
                GameObject clickedProject = FireRay();
                if (clickedProject!=null) {
                    if (clickedProject.name =="Project(Clone)") {

                        UnityEngine.Debug.Log(clickedProject.name);
                        //Change Mouse Pointer!
                        //Disable Canvas Graphics Raycaster?

                        setVerbsInPosition (clickedProject);
                        _selectProject.SetSelectedProject(clickedProject.transform);
                        _selectProject.SetProjectTextAndAnimation();
                    }
                }
            }
        }
    }

    void setVerbsInPosition(GameObject clickedProj) {
        
        string key = clickedProj.GetComponent<LookatCharacter>().thisProjectDetails._verbSet;
        int maxNum = (key.Length < bottomSlots.Length) ? key.Length : bottomSlots.Length;

        for (int i = 0; i < _selectProject.selectedVerbs.Length; i++) {
             _selectProject.selectedVerbs[i] = "empty";
        }

        for (int i = 0; i<maxNum; i++) {

            //Decode first Verb to Find
            char c = key[i];
            string toFind = "";
            if (c == 'R') toFind = "Research";
            else if (c == 'M') toFind = "Make";
            else if (c == 'A') toFind = "Analyze";
            else if (c == 'S') toFind = "Share";
            else if (c == 'D') toFind = "Dream";

            //Find that verb dropdown among verbSlots children
            for (int j = 0; j < 5; j++) {
                if (verbSlots[j].childCount>0) {
                    if (verbSlots[j].GetChild(0).gameObject.name == toFind) {
                        verbSlots[j].GetChild(0).position = bottomSlots[i].position;
                        verbSlots[j].GetChild(0).SetParent(bottomSlots[i]);
                    }
                }
            }

            _selectProject.selectedVerbs[i] = toFind;
        }

    }

    GameObject FireRay() {
        
        //Creates a Ray from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData)) {
            return hitData.transform.gameObject;
        }
        else 
            return null;
        
    }

}
