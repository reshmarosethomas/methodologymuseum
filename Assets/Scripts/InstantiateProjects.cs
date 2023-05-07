using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstantiateProjects : MonoBehaviour
{   
    private int projNum = 5;

    public class _Project {

        public Material _material;
        public string _verbSet;
        public string _projectName;
        public string _projectText;
        public string _artist;
        public int _projID;

        public _Project (int i, Material mat, string verbs, string name, string text, string artist) {
            _projID = i;
            _material = mat;
            _verbSet = verbs;
            _projectName = name;
            _projectText = text;
            _artist = artist;
        }  
    }

    //Stores Prefab
    public GameObject projectPrefab; 

    //Stores Instantiated Blocks
    GameObject[] projectBlocks;
    public static List<_Project> _projectsList = new List<_Project>();

    //Stores all project images & materials 
    public Material materialPrefab;

    Material[] projectImages = new Material[5];
    string[] projectTexts = new string[5];
    string[] projectTitles = new string[5];
    string[] projectVerbSets = new string[5]; 
    string[] projectArtists = new string[5]; 

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
        //Creates all the materials required for each project's image quads
        setImgTexturesToMaterials();

        //Stores (initializes) all the text for all projects (title, artist, text, verbSet) into arrays
        storeTextAndTitles();

        //Instatiate Projects in Position
        instantiateCubesInGrid();
        
        //Set properties of each instantiated project 
        setPropertiesOfEachProject();

    }

    // Update is called once per frame
    void Update()
    {   
        timer += Time.deltaTime;
        
        //Perlin movement applied to all projects that are children to ProjectHolder 
        unselectedPerlinMovement();
    }



    void unselectedPerlinMovement() {

        foreach (Transform child in transform)
        {   
            float x = child.localPosition.x;
            float y = child.localPosition.y;
            float z = child.localPosition.z;

            //Option 1: Using Translate
            child.Translate(Vector2.one * (Mathf.PerlinNoise((timer + z)*0.1f, x*0.1f)*0.01f - 0.005f));

            //Option 2: Using Lerp [Needs Fixing]
            //Vector3 newPos = new Vector3(x + (Mathf.PerlinNoise((timer)*0.1f, x*0.1f)*0.01f - 0.005f), y + (Mathf.PerlinNoise((timer)*0.1f, x*0.1f)*0.01f - 0.005f), z + (Mathf.PerlinNoise((timer)*0.1f, x*0.1f)*0.01f - 0.005f));
            //child.transform.position = Vector3.Lerp(transform.localPosition, newPos, 0.25f*currMs);
            
        }
    }

    void instantiateCubesInGrid() {
        for (int i = 0; i < noInRow; i++)
        {
            for (int j = 0; j < noInCol; j++)
            {   
                for (int k = 0; k < noInLin; k++) 
                {   
                    Vector3 projectPos = new(i * 3f - 16, j * 3f + 5, k * 3f - 18);
                    Instantiate(projectPrefab, projectPos, Quaternion.identity, transform);

                }
            }
        }
    }

    void setPropertiesOfEachProject() {
        int settingIndex = 0;
        foreach (Transform child in transform) 
        {   
            //_Project (int i, Material mat, string verbs, string name, string text, string artist)
            _projectsList.Add( new _Project (
                settingIndex, 
                projectImages[settingIndex%projNum], 
                projectVerbSets[settingIndex%projNum], 
                projectTitles[settingIndex%projNum], 
                projectTexts[settingIndex%projNum], 
                projectArtists[settingIndex%projNum]
            ));

            //Set Project Image on Quad
            Transform plane = child.Find("Quad");
            plane.GetComponent<MeshRenderer>().material = projectImages[settingIndex%projNum];

            //Set Verbs, Text, Title, Artist, ID
            child.gameObject.GetComponent<LookatCharacter>().thisProjectDetails = _projectsList[settingIndex];
            //UnityEngine.Debug.Log(child.gameObject.GetComponent<LookatCharacter>().thisProjectDetails._projectName);

            settingIndex ++;
        }
    }


    void setImgTexturesToMaterials() {

        for (int i = 0; i<projNum; i++) {
            
            //Load Image as Texture
            var bytes = System.IO.File.ReadAllBytes("Assets/Materials/Textures/p" + (i+1).ToString()+ ".png");
            var myTexture = new Texture2D(1, 1);
            myTexture.LoadImage(bytes);

            //Set Image-texture to Material
            Material x = new Material(materialPrefab); 
            x.mainTexture = myTexture;
            projectImages[i] = x;

        }

    }



    void storeTextAndTitles() {

        string dreamCol = "#FFD6001E"; //yellow
        string makeCol = "#FF5C001E"; //red
        string analyzeCol = "#26C9FF1E"; //blue
        string researchCol = "#2BFF4F1E"; //green
        string shareCol = "#AE0DFF1E"; //purple

        projectTitles[0] = "Folding Chess Set";
        projectArtists[0] = "Brian Ignaut";
        projectTexts[0] = $"Brian <mark={dreamCol}>imagined</mark> & <mark={makeCol}>built</mark> various origami mechanisms on wood and metal. He then <mark={analyzeCol}>applied</mark> the mechanisms on products where he <mark={researchCol}>discovered</mark> it enhanced function, to <mark={makeCol}>create</mark> unique instruments and tested & <mark={shareCol}>sold</mark> them.";
        projectVerbSets[0] = "DMARMS";

        projectTitles[1] = "Planetary Map of Fast Fashion";
        projectArtists[1] = "Saat Sahelian Collective";
        projectTexts[1] = $"Sumedha spent time <mark={researchCol}>researching</mark> a new community through long walks. After earning their trust, she <mark={makeCol}>facilitated</mark> a collective of women, who embroidered their personal narratives together. On <mark={analyzeCol}>synthesis of the socio-cultural context</mark> of the women, they <mark={dreamCol}>conceptualized</mark> and <mark={makeCol}>built</mark> this map of their locality. They <mark={shareCol}>carried this map out into the public to share</mark> their stories.";
        projectVerbSets[1] = "RMADMS";

        projectTitles[2] = "100 Days of Making Me";
        projectArtists[2] = "Carolina Della Valle";
        projectTexts[2] = $"Carolina spent 100 days <mark={dreamCol}>imagining, exploring,</mark> releasing, unwinding, <mark={makeCol}>making,</mark> and playing with different medium and tools that piqued her interest, and <mark={shareCol}>recorded and shared</mark> her work, in order to <mark={analyzeCol}>recognize</mark> what art style makes her come alive, which thereby is what the world needs - more people who feel alive.";
        projectVerbSets[2] = "DMSA";

        projectTitles[3] = "Eyewriter";
        projectArtists[3] = "TemptOne & Team";
        projectTexts[3] = $"The team <mark={researchCol}>researched</mark> the effects of the disease ALS (which left graffiti artist TemptOne completely paralyzed except for his eyes), <mark={dreamCol}>brainstormed</mark> and <mark={makeCol}>developed</mark> a low-cost, <mark={shareCol}>open source</mark> eye-tracking system that allows ALS patients to draw using just their eyes.";
        projectVerbSets[3] = "RADMS";

        projectTitles[4] = "Stor.io";
        projectArtists[4] = "Awanee Joshi";
        projectTexts[4] = $"Awanee started by <mark={researchCol}>spending time</mark> in her area of interest - communication through storytelling. She <mark={researchCol}>researched</mark> where and why people have difficulty presenting / communicating, and <mark={makeCol}>made a series of mini prototypes</mark> to <mark={analyzeCol}>test her assumptions and hypothesis</mark>. She then <mark={makeCol}>created</mark> the concept of stor.io platform that helps build compelling presentation structures through data-driven insights and reflection prompts. It went through multiple rounds of testing with users to refine features.";
        projectVerbSets[4] = "RAMRS";


    }

    
}


