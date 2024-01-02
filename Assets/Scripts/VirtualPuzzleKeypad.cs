using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPuzzleKeypad : MonoBehaviour
{
    public int padIndex;

    private void OnMouseDown()
    {
        this.transform.parent.GetComponent<TouchPuzzle>().InteractPuzzle(padIndex);
    }
}
