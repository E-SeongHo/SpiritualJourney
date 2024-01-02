using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPuzzle : MonoBehaviour
{
    public GameObject realPuzzle;
    private ST_PuzzleDisplay puzzleDisplay;

    void Start()
    {
        puzzleDisplay = realPuzzle.GetComponent<ST_PuzzleDisplay>();
    }

    public void InteractPuzzle(int index)
    {
        puzzleDisplay.MoveTile(index);
    }
}
