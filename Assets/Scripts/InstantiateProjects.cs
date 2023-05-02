using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstantiateProjects : MonoBehaviour
{   

    public class _Project {

        Material _material;
        string _verbSet;
        string _projectName;
        string _projectText;

        _Project(Material mat, string verbs, string name, string text) {
            _material = mat;
            _verbSet = verbs;
            _projectName = name;
            _projectText = text;
        }  
    }

    //Stores Prefab
    public GameObject projectPrefab; 

    //Stores Instantiated Blocks
    GameObject[] projectBlocks;

    //Stores all project images & Verb Code Sets 
    public Material projectMatPrefab;
    public string[] projectVerbSet = new string[4]; 
    
    private Material[] _projectImages = new Material[4];
    public List<_Project> _projectsList = new List<_Project>();
    // List<_Project> _projects = new List<_Project> {
    //     new _Project{_material = "", _verbSet = "Scott", _projectName = "Gurthie", _projectText = "Gurthie"},
    //     new _Project{ID = 2, FirstName = "Bill", LastName = "Gates"}
    // };


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
        for (int i = 0; i<4; i++) {
            
            var bytes = System.IO.File.ReadAllBytes("Assets/Materials/Textures/p" + (i+1).ToString()+ ".png");
            var myTexture = new Texture2D(1, 1);
    
            myTexture.LoadImage(bytes);
            Material x = new Material(projectMatPrefab); 
            x.mainTexture = myTexture;
            _projectImages[i] = x;
        }

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
            plane.GetComponent<MeshRenderer>().material = _projectImages[projIndex%4];

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


