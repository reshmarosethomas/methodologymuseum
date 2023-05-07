using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSounds : MonoBehaviour
{   
    public AudioSource bgm;
    // Start is called before the first frame update
    void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.Play();
    }

}
