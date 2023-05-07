using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class selectProject : MonoBehaviour
{   
    //private int projNum = 4;

    //Waypoint from Camera Child Transform (to position selected project)
    public Transform waypoint;

    //Text object to display the project text in
    public TextMeshProUGUI ProjectTextDisplay;
    public TextMeshProUGUI ProjectTitleDisplay;

    //Stores all panels & slots
    Transform[] slots = new Transform[4];
    GameObject bottomPanel;
    GameObject verbPanel;
    GameObject baseOverlay;
    GameObject clearButton;

    //Stores slot children's verb types (containing verbs!)
    public string[] selectedVerbs = new string[4]; //constantly updates
    string[] prevVerbs = new string[4]; 

    //To manage StopWatch
    Stopwatch sw = new Stopwatch();
    float period = 1000;
    float prevMs = 0;
    float currMs = 0;

    InstantiateProjects instantiateProjects;

    //Flags when a project is in focus
    public bool projInFocus = false;

    //Flags when there is no proj for a specific key created by the user
    bool noProjForKey = false;
    public bool projChange = false;
    bool displayClearButton = false;
    bool selectAnimate = false; //if the selected project is already animated

    //Stores the current project in focus, prev proj in focus, and their positions
    public GameObject selectedChild;
    GameObject prevSelectedChild;
    public Vector3 originalPosOfSelected;
    Vector3 originalPosOfPrevSelected;


    // Start is called before the first frame update
    void Start()
    {   
        ProjectTextDisplay.text = "What makes an appropriate design process?";

        clearButton = GameObject.Find("Canvas/ClearButton");
        bottomPanel = GameObject.Find("Canvas/BottomPanel");
        verbPanel = GameObject.Find("Canvas/VerbPanel");
        baseOverlay = GameObject.Find("Canvas/Base");
        baseOverlay.SetActive(false);

        instantiateProjects = gameObject.GetComponent<InstantiateProjects>();

        for (int i = 0; i<4; i++) {
            selectedVerbs[i] = "empty";
            prevVerbs[i] = "empty";
            slots[i] = bottomPanel.transform.GetChild(i).GetComponent<Transform>();
        }

        sw.Start();
    }

    // Update is called once per frame
    void Update()
    {   
        currMs = sw.ElapsedMilliseconds;

        if (currMs - prevMs >= period) {
            prevMs = currMs;
            
            if (checkChangesInSlots() && !selectAnimate) {

                string key = checkVerbOrder();
                if (key!="orderUnchanged") {
                    selectProjectBlock(key);
                    SetProjectTextAndAnimation();
                }
            }
        }

        if (displayClearButton)
            clearButton.SetActive(true);
        else
            clearButton.SetActive(false);
        
    }

    public void clearAllSlots() {

        for (int i = 3; i >= 0; i--) {

            if (slots[i].gameObject.activeSelf) {

                if (slots[i].childCount > 1) {

                    Transform freeSlotInVerbPanel = findEmptySlotInVerbPanel();
                    if (freeSlotInVerbPanel!=null) {
                        slots[i].GetChild(1).position = freeSlotInVerbPanel.position;
                        slots[i].GetChild(1).SetParent(freeSlotInVerbPanel);
                    }
                    else {
                        UnityEngine.Debug.Log("Error: No empty slot!");
                    }
                }
            }
        } 

        for (int i = 0; i<4; i++) {
            selectedVerbs[i] = "empty";
            prevVerbs[i] = "empty";
        }

        selectProjectBlock("");
        SetProjectTextAndAnimation();
        selectAnimate= false;
        
    }

    Transform findEmptySlotInVerbPanel() {
        foreach (Transform child in verbPanel.transform) {
            if (child.childCount == 0)
                return child;
        }

        return null;
    }

    bool checkChangesInSlots() {

        bool change = false;
        displayClearButton = false;
        //Check if it changed since the last time
        //If changed, call for a change in project blocks
        //selectedVerbs continuously updates based on selected verbs by the user 

        for (int i = 0; i < 4; i++) 
        {
            //Check if slot is active
            if (slots[i].gameObject.activeSelf) {

                //Check if slot has a child
                if (slots[i].childCount > 1) {

                    //Check if selectedVerbs changed & if not change it
                    string verbTypeInSlot = slots[i].GetChild(1).gameObject.name;

                    if (verbTypeInSlot != selectedVerbs[i]) 
                    {   
                        //UnityEngine.Debug.Log("verb added");
                        selectedVerbs[i] = verbTypeInSlot;
                        change = true;
                        // checkVerbOrder();
                    }

                    displayClearButton = true;
                    
                }

                else {
                    
                    //Check if selectedVerbs is empty & if not, empty it
                    if (selectedVerbs[i]!= "empty") 
                    {
                        //UnityEngine.Debug.Log("slot is emptied");
                        selectedVerbs[i] = "empty";
                        change = true;
                        //checkVerbOrder();
                    }
                }

            }
        }

        return change;
    }

    string checkVerbOrder() {
        
        //stores the changed selected verbs after removing the in-between 'empty's
        string[] tempVerbOrder = new string[] {"empty", "empty", "empty", "empty"};

        for (int i = 0, j = 0; i<4; i++) {
            if (selectedVerbs[i]!="empty") {
                tempVerbOrder[j] = selectedVerbs[i];
                j++;
            }
        }

        //checks if normalized selectedVerbs (stored in tempVerbOrder), is the same as prevVerbs, 
        //and if not, make them equal and change the selected-project object by calling modify block
        if (!areArraysEqual(tempVerbOrder, prevVerbs)) {

            string projectKey = ""; //stores key that is passed to modify project

            for (int i = 0; i < 4; i++) 
            {
                prevVerbs[i] = tempVerbOrder[i];
                string s = prevVerbs[i];
                char c = s[0];
                if (c!='e')
                    projectKey += c;
            }
            
            UnityEngine.Debug.Log(projectKey);
            //selectProjectBlock(projectKey);
            return projectKey;
        }

        string returnMessage = "orderUnchanged";
        return returnMessage;
    }

    bool areArraysEqual(string[] firstArray, string[] secondArray) {
    
        if (firstArray.Length != secondArray.Length)
            return false;

        for (int i = 0; i < firstArray.Length; i++)
        {
            if (firstArray[i] != secondArray[i])
                return false;
        }
        return true;
    }


    void selectProjectBlock(string key) {

        projInFocus = false;
        noProjForKey = false;
        projChange = false;

        if (key!="") {

            List<int> projIndices = new List<int>();    //stores indices of all projects that match key

            for (int i = 0; i < InstantiateProjects._projectsList.Count; i++) {
                
                string verbSet = InstantiateProjects._projectsList[i]._verbSet;
                bool isMatch = false;
                
                //If key is shorter than or equal to the verbSet of this project
                if (verbSet.Length >= key.Length) {
                    for (int j = 0, k = 0; j < verbSet.Length; j++) {
                        if (verbSet[j]==key[k]) {
                            k++;
                            if (k >= key.Length) {
                                isMatch = true;
                                j = verbSet.Length;
                            }
                        }
                    }
                }

                if (isMatch) {
                    projIndices.Add(i);
                }

            }

            if (projIndices.Count >= 1) {

                int dice = Random.Range(0, projIndices.Count);
                int selectedID = projIndices[dice];

                foreach (Transform child in transform) {

                    int x = child.GetComponent<LookatCharacter>().thisProjectDetails._projID;

                    if (x == selectedID) {
                        
                        SetSelectedProject(child);
                    }

                }

                if (!projInFocus) {
                    if (selectedChild.GetComponent<LookatCharacter>().thisProjectDetails._projID == selectedID) {
                        projInFocus = true;
                        projChange = false;
                    }
                }
                
            }

            else if (projIndices.Count < 1) {
                //No project fits this key
                //No project will be in focus
                noProjForKey = true;
                projInFocus = false;

                if (selectedChild!=null)
                    projChange = true;

            }

        }

        //what happens if No key has been created by the user (the key is "")
        if (key == "") {
            
            projInFocus = false;

            if (selectedChild!=null) {
                projChange = true; 
                //UnityEngine.Debug.Log("No key created");
            }
            
        }
        
    }

    public void SetSelectedProject(Transform project) {
        
        //Sets existing selected project as prevSelected, if any
        if (selectedChild!=null) {
            prevSelectedChild = selectedChild;
            originalPosOfPrevSelected = originalPosOfSelected;
        }   

        //Sets project passed as selectedChild & Stores its position
        selectedChild = project.gameObject;
        originalPosOfSelected = project.position;

        //Sets projInFocus & ProjChange flags
        projInFocus = true;
        projChange = true;
    }

    public void SetProjectTextAndAnimation() {
        //POSITION PROJECTS (AND PUT BACK PRE-POSITIONED PROJECTS)
        //SET TEXT & BASE ALPHA
        selectAnimate = true;
        if (projInFocus) { 

            if (projChange) {

                //Animate Project to the Player & Set Parent
                StartCoroutine(LerpPosition(selectedChild, originalPosOfSelected, waypoint.position, waypoint));
                //selectedChild.transform.SetParent(waypoint);
                //UnityEngine.Debug.Log(selectedChild.transform.parent.name);

                //Animate Prev Project Away
                if (prevSelectedChild!=null) {
                    StartCoroutine(LerpPosition(prevSelectedChild, prevSelectedChild.transform.position, originalPosOfPrevSelected, transform));
                    //prevSelectedChild.transform.SetParent(transform);
                    prevSelectedChild = null; //Release pointer to the prev project
                }

            }

            ProjectTextDisplay.text = selectedChild.GetComponent<LookatCharacter>().thisProjectDetails._projectText;
            ProjectTitleDisplay.text = selectedChild.GetComponent<LookatCharacter>().thisProjectDetails._projectName;
            baseOverlay.SetActive(true);
            
        }

        if (!projInFocus) {
            
            if (projChange) {

                //Animate Project to the Player
                StartCoroutine(LerpPosition(selectedChild, selectedChild.transform.position, originalPosOfSelected, transform));

                //Set Project as Child of Project Holder
                //selectedChild.transform.SetParent(transform);

                //Release pointer to the project
                selectedChild = null;
                
            }

            
            if (noProjForKey) {
                ProjectTextDisplay.text = "This collection doesn't have a project for this process yet! (The key word here is 'yet').";
            }
            else {
                ProjectTextDisplay.text = "What makes an appropriate design process?";
            }

            ProjectTitleDisplay.text = "METHODOLOGY MAKER";
            baseOverlay.SetActive(false);
        }
    }

    IEnumerator LerpPosition(GameObject projBlock, Vector3 startPos, Vector3 endPos, Transform parentToSet) {

        float timeElapsed = 0f;
        float lerpDuration = 1.0f; 

        while (timeElapsed < lerpDuration)
        {
            projBlock.transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        projBlock.transform.position = endPos;
        projBlock.transform.SetParent(parentToSet);
        selectAnimate = false;

    }

}
