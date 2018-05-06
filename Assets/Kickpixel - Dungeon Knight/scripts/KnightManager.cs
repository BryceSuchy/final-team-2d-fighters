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
        //this code only affects the animations. It does nothing to the actual functionality of the knight (see Player.cs)
        //back
            if (Input.GetKeyDown(KeyCode.W))
            {
                //the walk values correlate the animation we want to use (like walkvalue 2 correlates to the animation for walking up the screen)
                anim.SetInteger("WalkValue",2);
            }
            //attack
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                anim.SetInteger("WalkValue", 6);
            }
        //front
            //walk
            if (Input.GetKeyDown(KeyCode.S))
            {
                anim.SetInteger("WalkValue", 0);
            }
            //Attack
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                anim.SetInteger("WalkValue", 4);
            }
        //right
            if (Input.GetKeyDown(KeyCode.D))
            {
                anim.SetInteger("WalkValue", 3);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                anim.SetInteger("WalkValue", 7);
            }
        //left
            if (Input.GetKeyDown(KeyCode.A))
            {
                anim.SetInteger("WalkValue", 1);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                anim.SetInteger("WalkValue", 5);
            }


    }
}
