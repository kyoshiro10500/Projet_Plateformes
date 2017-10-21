using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyPlatform : MonoBehaviour {

    [SerializeField]
    private bool isGoThrough;

    [SerializeField]
    private bool isKillPlayer;

    public bool IsGoThrough
    {
        get
        {
            return isGoThrough;
        }
    }

    public bool IsKillPlayer
    {
        get
        {
            return isKillPlayer;
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
