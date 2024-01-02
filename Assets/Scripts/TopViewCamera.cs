using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewCamera : MonoBehaviour
{
    private Camera cam;
    public MazeSpawner maze;
    public int wallThickness = 5;

    private void Start()
    {
        cam = gameObject.transform.GetComponent<Camera>();
        int gap = -((int)maze.CellWidth/2 + wallThickness);
        int viewWidth = (int)maze.CellWidth * maze.Columns + 2*wallThickness;
        Vector3 startPoint = new Vector3(gap, 200, gap);

        cam.transform.position = startPoint + new Vector3(viewWidth / 2, 200, viewWidth / 2);
        cam.orthographicSize = viewWidth / 2;

        CullingPoneglyph();
    }

    public void CullingPoneglyph()
    {
        GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("Poneglyph"));
    }

}
