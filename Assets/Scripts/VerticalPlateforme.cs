using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlateforme : MonoBehaviour {

    [SerializeField]
    private float max_amplitude;

    [SerializeField]
    private float vitesse;

    [SerializeField]
    private bool is_horizontal;

    private float min_hauteur;
	// Use this for initialization
	void Start () {
        if(is_horizontal)
        {
            min_hauteur = this.transform.position.x - 0.01f;
        }
        else
        {
            min_hauteur = this.transform.position.y - 0.01f;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        if(is_horizontal)
        {
            if (this.transform.position.x <= min_hauteur || this.transform.position.x >= min_hauteur + max_amplitude)
            {
                vitesse = -vitesse;
            }

            this.transform.Translate(new Vector3(vitesse, 0, 0));
        }
        else
        {
            if (this.transform.position.y <= min_hauteur || this.transform.position.y >= min_hauteur + max_amplitude)
            {
                vitesse = -vitesse;
            }

            this.transform.Translate(new Vector3(0, vitesse, 0));
        }
	}
}
