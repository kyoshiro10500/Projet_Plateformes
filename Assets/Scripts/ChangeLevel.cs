using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevel : MonoBehaviour {


    [SerializeField]
    private int nbLevel;



    public int NbLevel
    {
        get
        {
            return nbLevel;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // TODO : Si il y a collision, on change de scene/niveau
        Application.LoadLevel("Scene" + NbLevel);
    }
}
