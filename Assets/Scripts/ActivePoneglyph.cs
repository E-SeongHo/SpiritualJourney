using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePoneglyph : MonoBehaviour
{
    // for debuging
    public bool active = false;
    public bool tried = false;
    
    public GameObject puzzle;
    public GameObject keypad;

    private void Update()
    {
        if (active && !tried)
        {
            ActivatePoneglyph();
            tried = true;
        }
    }
    public void ActivatePoneglyph()
    {
        puzzle.GetComponent<ST_PuzzleDisplay>().Init();
        keypad.transform.gameObject.SetActive(true);
    }

}
