using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatCharacter : MonoBehaviour
{   
    private Transform player;
    public string thisProjectVerbs = "hi ";
    public static string test = "test";

    void Start() 
    {
        player = GameObject.Find("PlayerCapsule").GetComponent<Transform>();
    }

    void Update()
    {   
        if (transform.parent.gameObject.name=="ProjectHolder")
        transform.LookAt(player);
    }

    
}
