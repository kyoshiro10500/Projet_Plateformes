﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal_axis = 0f;
        horizontal_axis = Input.GetAxisRaw("Horizontal");
        if(horizontal_axis != 0)
        {
            this.GetComponent<GravityManager>().Horizontal_Move(horizontal_axis);
        }

        this.GetComponent<GravityManager>().Set_Dash(Input.GetButton("Dash"));


        if(Input.GetButtonDown("Jump"))
        {
            this.GetComponent<GravityManager>().Set_Jump(true);
        }

        if(Input.GetButtonUp("Jump"))
        {
            this.GetComponent<GravityManager>().Set_Jump(false);
        }
    }
}
