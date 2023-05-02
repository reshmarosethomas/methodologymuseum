using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class selectProject : MonoBehaviour
{   
    //Waypoint from Camera Child Transform
    public Transform waypoint;

    //Text object to display the project text in
    public TextMeshProUGUI ProjectTextDisplay;

    //Stores all project text
    public string[] projectText = new string[4];

    //Stores all bottom panel slots
    public Transform[] slots = new Transform[4];
    public GameObject bottomPanel;

    //Stores slot children's verb types (containing verbs!)
    string[] selectedVerbs = new string[4]; //constantly updates
    string[] prevVerbs = new string[4]; 

    //To manage StopWatch
    Stopwatch sw = new Stopwatch();
    float period = 500;
    float prevMs = 0;
    float currMs = 0;

    public InstantiateProjects findProj;

    //Flags when a project is in focus
    bool projInFocus = false;

    //Stores the project in focus
    GameObject selectedChild;
    Vector3 originalPosOfSelected;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i<4; i++) {
            selectedVerbs[i] = "empty";
            prevVerbs[i] = "empty";
        }

        bottomPanel = GameObject.Find("Canvas/BottomPanel");

        slots[0] = bottomPanel.transform.GetChild(0).GetComponent<Transform>();
        slots[1] = bottomPanel.transform.GetChild(1).GetComponent<Transform>();
        slots[2] = bottomPanel.transform.GetChild(2).GetComponent<Transform>();
        slots[3] = bottomPanel.transform.GetChild(3).GetComponent<Transform>();

        findProj = gameObject.GetComponent<InstantiateProjects>();
        //UnityEngine.Debug.Log(findProj.projectVerbSet[0]);

        sw.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        currMs = sw.ElapsedMilliseconds;

        if (currMs - prevMs >= period) {
            prevMs = currMs;
            checkChangesInSlots();
        }

        if (projInFocus) {
            // UnityEngine.Debug.Log("lerping child");
            selectedChild.transform.position = Vector3.Lerp(transform.position, waypoint.position, 0.25f*currMs);
            // selectedChild.transform.position = waypoint.position;
        }

        // if (!projInFocus) {
        //     selectedChild.transform.position = Vector3.Lerp(transform.position, waypoint.position, 5f*currMs);
        //     selectedChild.transform.position = originalPosOfSelected;
        //     selectedChild.transform.SetParent(transform);
        // }
        
    }

    void checkChangesInSlots() {

        //Check if it changed since the last time
        //If changed, call for a change in project blocks

        //selectedVerbs can continuously update selected verbs by the user 
        //selectedVerbs need to be parsed into a series of verbs to use to call projects

        for (int i = 0; i < 4; i++) 
        {
            //Check if slot is active
            if (slots[i].gameObject.activeSelf) {

                //Check if slot has a child
                if (slots[i].childCount > 0) {

                    //Check if selectedVerbs changed & if not change it
                    string verbTypeInSlot = slots[i].GetChild(0).gameObject.name;

                    if (verbTypeInSlot != selectedVerbs[i]) 
                    {   
                        //UnityEngine.Debug.Log("verb added");
                        selectedVerbs[i] = verbTypeInSlot;
                        checkVerbOrder();
                    }
                    
                }
                else {
                    
                    //Check if selectedVerbs is empty & if not, empty it
                    if (selectedVerbs[i]!= "empty") 
                    {
                        //UnityEngine.Debug.Log("slot is emptied");
                        selectedVerbs[i] = "empty";
                        checkVerbOrder();
                    }
                }

            }
        }
    }

    void checkVerbOrder() {
        
        //stores the changed selected verbs after removing the in-between 'empty's
        string[] tempVerbOrder = new string[] {"empty", "empty", "empty", "empty"};

        for (int i = 0, j = 0; i<4; i++) {
            if (selectedVerbs[i]!="empty") {
                tempVerbOrder[j] = selectedVerbs[i];
                j++;
            }
        }

        //checks if normalized selectedVerbs (stored in tempVerbOrder), is the same as prevVerbs, 
        //and if not, make them equal and change the selected projects
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
            modifyProjectBlock(projectKey);
        }
    
    }

    bool areArraysEqual(string[] firstArray, string[] secondArray)
    {
        if (firstArray.Length != secondArray.Length)
            return false;
        for (int i = 0; i < firstArray.Length; i++)
        {
            if (firstArray[i] != secondArray[i])
                return false;
        }
        return true;
    }

    void modifyProjectBlock(string key) {
        int[] Indices = new int[3];
        int options = 0;
        //GetComponent<InstantiateProjects>().FindProject(key);
        //findProj.FindProject(key);

        // for (int i=0; i<4; i++) {

        //     string word = GetComponent<InstantiateProjects>().projectVerbSet[i];
        //     for (int j=0; j<word.Length; j++) {
                
        //         if (word[j] == key[0]) {
        //                 Indices[options] = i; options++;
        //         }
        //         if (key.Length == 1) {
        //             if (word[j] == key[0]) {
        //                 Indices[options] = i; options++;
        //             }
        //         } else if (key.Length >=2) {
        //             if (word[j] == key[0]) {
        //                 Indices[options] = i; options++;
        //             }
        //         }

        //         for (int k=0; k<key.Length; k++) {
        //         }
        //     }
        // }

        if (key!="") {
            if (key[0] == 'M')
            Indices[0] = 0;
            else if (key[0]== 'R')
            Indices[0] = 1;
            else if (key[0]== 'D')
            Indices[0] = 2;

            foreach (Transform child in transform)
            {   
                if (Indices[0] == options) {
                    selectedChild = child.gameObject;
                    originalPosOfSelected = child.position;
                    projInFocus = true;
                    child.SetParent(waypoint);
                    UnityEngine.Debug.Log(child.gameObject.name);
                }
                options++;
                // float x = child.localPosition.x;
                // float y = child.localPosition.y;
                // float z = child.localPosition.z;
                // child.Translate(Vector2.one * (Mathf.PerlinNoise((timer + z)*0.1f, x*0.1f)*0.01f - 0.005f));
                //USE LERP!!! 
            }
        } 

        else {
            projInFocus = false;
        }
        
        if (projInFocus) {
            UnityEngine.Debug.Log("lerping child");
            selectedChild.transform.position = Vector3.Lerp(transform.position, waypoint.position, 5f*Time.deltaTime);
        }

        if (!projInFocus) {
            selectedChild.transform.SetParent(transform);
            selectedChild.transform.position = Vector3.Lerp(transform.position, originalPosOfSelected, 5f*currMs);
            //selectedChild.transform.position = originalPosOfSelected;
        }

        //POSITION PROJECTS (AND PUT BACK PRE-POSITIONED PROJECTS)
        //SET TEXT & BASE ALPHA
    }

    void SetProjectText() {

    }

}
