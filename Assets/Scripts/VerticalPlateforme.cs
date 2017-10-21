﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlateforme : MonoBehaviour {

    [SerializeField]
    private float max_amplitude;

    [SerializeField]
    private float vitesse;

    private float vitesse_horizontale = 0;
    public float Vitesse_horizontale
    {
        get
        {
            return vitesse_horizontale;
        }
       
    }

    private float vitesse_verticale = 0;
    public float Vitesse_verticale
    {
        get
        {
            return vitesse_verticale;
        }
    }

    [SerializeField]
    private bool is_horizontal;

    private float min_hauteur;
	// Use this for initialization
	void Start () {
        if(is_horizontal)
        {
            min_hauteur = this.transform.position.x - 0.01f;
            vitesse_horizontale = vitesse;
        }
        else
        {
            min_hauteur = this.transform.position.y - 0.01f;
            vitesse_verticale = vitesse;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        if(is_horizontal)
        {
            if (this.transform.position.x <= min_hauteur)
            {
                vitesse = -vitesse;
                vitesse_horizontale = vitesse;
                this.transform.position = new Vector3(min_hauteur, this.transform.position.y, this.transform.position.z);
            }
            else if (this.transform.position.x > min_hauteur + max_amplitude)
            {
                vitesse = -vitesse;
                vitesse_horizontale = vitesse;
                this.transform.position = new Vector3(min_hauteur + max_amplitude,this.transform.position.y, this.transform.position.z);
            }

            this.transform.Translate(new Vector3(Time.deltaTime * vitesse, 0, 0));
        }
        else
        {
            if (this.transform.position.y < min_hauteur)
            {
                vitesse = -vitesse;
                vitesse_verticale = vitesse;
                this.transform.position = new Vector3(this.transform.position.x, min_hauteur, this.transform.position.z);
            }
            else if (this.transform.position.y > min_hauteur + max_amplitude)
            {
                vitesse = -vitesse;
                vitesse_verticale = vitesse;
                this.transform.position = new Vector3(this.transform.position.x, min_hauteur+max_amplitude, this.transform.position.z);
            }
            this.transform.Translate(new Vector3(0, Time.deltaTime * vitesse, 0));
        }
	}
}
