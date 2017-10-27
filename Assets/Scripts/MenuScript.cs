using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("test");
        Application.LoadLevel("Scene1");
    }
}
