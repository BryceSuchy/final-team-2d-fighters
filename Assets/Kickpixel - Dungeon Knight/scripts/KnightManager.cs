using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightManager : MonoBehaviour {
    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("WalkValue",2);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            anim.SetInteger("WalkValue", 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("WalkValue", 3);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("WalkValue", 1);
            
        }
        if (Input.GetKeyDown(KeyCode.Space) && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
        {
            anim.SetInteger("WalkValue", 4);

        }

    }
}
