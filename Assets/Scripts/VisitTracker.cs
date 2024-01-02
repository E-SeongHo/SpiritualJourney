using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisitTracker : MonoBehaviour
{
    private int row;
    private int column;
    private bool visited = false;
    public GameObject visitedMarker;
    public GameObject pathFindingMarker;

    public void SetRowColumn(int r, int c)
    {
        row = r;
        column = c;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        PathFinder.Instance.CurrentCell = (row, column);

        if (!visited)
        {
            visited = true;
            visitedMarker.SetActive(true);
        }
    }

    public void PathFinding()
    {
        StartCoroutine(TurnOnLight());
    }

    private IEnumerator TurnOnLight()
    {
        pathFindingMarker.SetActive(true);
        yield return new WaitForSeconds(5f);

        pathFindingMarker.SetActive(false);
    }
}
