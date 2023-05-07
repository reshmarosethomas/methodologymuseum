using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class activateSlots : MonoBehaviour
{   
     public Transform[] slots = new Transform[4];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i<4; i++) {
            slots[i] = transform.GetChild(i).GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {   
        int filledNum = 0;
        //List<int> slotIndices = new List<int>();

        for (int i = 0; i<slots.Length; i++) {

            if (slots[i].childCount <= 1) {
                slots[i].GetComponent<snapToSlot>().isFilled = false;
            } else {
                slots[i].GetComponent<snapToSlot>().isFilled = true;
                if (i!=slots.Length-1)
                    slots[i+1].gameObject.SetActive(true);
                filledNum ++;
                //slotIndices.Add(i);
            }
        }

        if (filledNum<3) {
            if (!slots[2].GetComponent<snapToSlot>().isFilled && !slots[3].GetComponent<snapToSlot>().isFilled) {
                slots[3].gameObject.SetActive(false);
                if (!slots[1].GetComponent<snapToSlot>().isFilled) {
                    slots[2].gameObject.SetActive(false);
                    if (!slots[0].GetComponent<snapToSlot>().isFilled)
                        slots[1].gameObject.SetActive(false);
                }
            }
        }
    }
}
