using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstantiateProjects : MonoBehaviour
{   
    //Stores Prefab
    public GameObject projectPrefab; 

    //Stores Instantiated Blocks
    GameObject[] projectBlocks;

    //Stores all project images & Verb Code Sets 
    public Material[] projectImages = new Material[4];
    public string[] projectVerbSet = new string[4]; 

    //Stories indices to traverse
    int projIndex = 0;

    //To instantiate projects in  a grid
    int noInRow = 5;
    int noInCol = 5;
    int noInLin = 5;

    //int numOfProjects = 125; 

    //Keeps track of time to float project blocks based on Perlin Noise
    float timer = 0;
  
    // Start is called before the first frame update
    void Start()
    {   
        //Instatiate Projects in Position
        for (int i = 0; i < noInRow; i++)
        {
            for (int j = 0; j < noInCol; j++)
            {   
                for (int k = 0; k < noInLin; k++) 
                {   
                    Vector3 projectPos = new(i * 3f - 16, j * 3f + 5, k * 3f - 18);
                    Instantiate(projectPrefab, projectPos, Quaternion.identity, transform);
                    //projectBlocks[projIndex] = Instantiate(projectPrefab, projectPos, Quaternion.identity, transform);
                    projIndex++;
                    //projectBlocks[(i*)+ (j*noInLin) + k] = Instantiate(projectPrefab, projectPos, Quaternion.identity, transform);
                }
            }
        }

        //UnityEngine.Debug.Log(projIndex);
        projIndex = 0;

        foreach (Transform child in transform) 
        {
            //Set Project Image on Quad
            Transform plane = child.Find("Quad");
            plane.GetComponent<MeshRenderer>().material = projectImages[projIndex%4];

            //Set Verbs
            child.gameObject.GetComponent<LookatCharacter>().thisProjectVerbs = projectVerbSet[projIndex%4]; 
            projIndex ++;
        }
        projIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        timer += Time.deltaTime;
        unselectedPerlinMovement();
    }

    void unselectedPerlinMovement() {
        foreach (Transform child in transform)
        {   
            float x = child.localPosition.x;
            float y = child.localPosition.y;
            float z = child.localPosition.z;
            child.Translate(Vector2.one * (Mathf.PerlinNoise((timer + z)*0.1f, x*0.1f)*0.01f - 0.005f));
            //USE LERP!!! 
        }
    }

    public void FindProject(string key) {

        // for (int i = 0; i<projectVerbSet.Length; i++) {
        //     if ()
        // }

        //SELECT PROJECTS
        //We have a set of four verbs in order [a, b, c, d]
        //We have a set of projects which may have 4 verbs in order
            //P1 b, a, c, d
            //P2 a, c, b, d
            //P3 a, b, d, c
            //P4 c, a, b, d ...
        //to match projects, we are going to look for sequence pairs

    }

    
}


