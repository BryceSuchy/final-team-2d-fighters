using Completed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestManager : MonoBehaviour {
    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //if(Player.instance.isChestOpen == true)
        if(0==1)
        {
            anim.SetTrigger("OpenChest");
        }
	}
}
