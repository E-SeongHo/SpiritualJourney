using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public GameObject manager;
    public bool activate = false;
    public bool tried = false;

    private void Start()
    {
        manager = GameObject.Find("GameManager");
    }
    private void Update()
    {
        if(!tried && activate)
        {
            tried = true;
            ActivateButton();
        }
    }

    public void ActivateButton()
    {
        manager.GetComponent<GameManager>().ExitMaze();
    }
}
