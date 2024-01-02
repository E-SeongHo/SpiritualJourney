using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInfo
{
    // for Astar
    public CellInfo(MazeCell c, int row, int column, int dep, int dist)
    {
        cell = c;
        y = row;
        x = column;
        depth = dep;
        evaluation = depth + 50 * dist;
    }
    // for Dijkstra
    public CellInfo(MazeCell c, int row, int column, int dep)
    {
        cell = c;
        y = row;
        x = column;
        depth = dep;
        evaluation = depth;
    }
    public int y;
    public int x;
    public int depth;
    public int evaluation;
    public CellInfo parent;

    public MazeCell cell;
}

class PriorityQueue
{
    public PriorityQueue(int size)
    {
        if (size < 50) size = 50;
        m_data = new CellInfo[size + 1];
    }

    public void Enqueue(CellInfo cell)
    {
        int current = ++m_size;
        m_data[current] = cell;

        while(current != rootIdx)
        {
            int parent = current / 2;
            if (m_data[parent].evaluation > m_data[current].evaluation)
            {
                (m_data[parent], m_data[current]) = (m_data[current], m_data[parent]);
                current = parent;
            }
            else break;
        }
    }

    public CellInfo Dequeue()
    {
        if (m_size == 0) return null;

        CellInfo ret = m_data[1];
        m_data[1] = m_data[m_size--];

        int current = rootIdx;
        while (current * 2 <= m_size) 
        {
            int left = current * 2;
            int right = current * 2 + 1;
            int child = (m_data[left].evaluation > m_data[right].evaluation)
                && right <= m_size ? right : left;

            if (m_data[current].evaluation > m_data[child].evaluation)
            {
                (m_data[current], m_data[child]) = (m_data[child], m_data[current]);
                current = child;
            }
            else break;
        }

        return ret;
    }

    public bool IsEmpty()
    {
        if (m_size == 0) return true;
        else return false;
    }

    public void Print()
    {
        Debug.Log("Print");
        for(int i = rootIdx; i <= m_size; i++)
        {
            Debug.Log(i + " " + m_data[i].evaluation);
        }
    }

    private CellInfo[] m_data;
    private int m_size = 0;
    private int rootIdx = 1;
}


public class PathFinder : MonoBehaviour
{
    private static PathFinder instance = null;

    public static PathFinder Instance { get { return instance; } }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public bool activate = false;
    public bool tried = false;

    private MazeSpawner spawner;
    private MazeCell[,] maze;
    public int rows;
    public int columns;

    private (int, int) currentCell; // row, column of current cell
    public (int, int) CurrentCell { get { return currentCell; } set { currentCell = value; } }

    private void OnEnable()
    {
        spawner = gameObject.GetComponent<MazeSpawner>();

        maze = spawner.GetMazeInfo();
        rows = maze.GetLength(0);
        columns = maze.GetLength(1);

        currentCell = (0, 0);
    }

    private void Update()
    {
        if(activate && !tried)
        {
            Debug.Log("Find Path");
            tried = true;
            FindPath();
        }
    }

    public void FindPath()
    {
        StartCoroutine(FindAstarPath());
    }

    // Heuristic evaluation For make player get an abstract way to exit
    // Show all process of selecting nodes
    public IEnumerator FindAstarPath()
    {
        PriorityQueue pq = new PriorityQueue(rows * columns * 2);
        int[,] evals = new int[rows, columns];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                evals[i, j] = int.MaxValue;

        int currentY = currentCell.Item1;
        int currentX = currentCell.Item2;
        int targetY = rows - 1;
        int targetX = columns - 1;
        int count = 0;
        CellInfo startCell = new CellInfo(maze[currentY, currentX], currentY, currentX, 0, GetManhattanDist(currentY, currentX, targetY, targetX));
        pq.Enqueue(startCell);
        evals[currentY, currentX] = startCell.evaluation;

        while (!pq.IsEmpty())
        {
            CellInfo current = pq.Dequeue();
            if (current == null)
            {
                Debug.Log("null");
                break;
            }
            GameObject floor = spawner.floorObjects[current.y, current.x];
            floor.GetComponent<VisitTracker>().PathFinding();
            
            if (current.cell.IsExit) break;
            count++;
            Debug.Log(count + " " + current.y + " " + current.x + " " + current.depth + " " + current.evaluation);
            yield return new WaitForSeconds(0.1f);

            CellInfo temp;
            int ny, nx, dist;
            // move right
            if (!current.cell.WallRight && !maze[current.y, current.x + 1].WallLeft) 
            {
                ny = current.y;
                nx = current.x + 1;
                dist = GetManhattanDist(ny, nx, targetY, targetX);
                if (evals[ny, nx] > dist + current.depth + 1)
                {
                    evals[ny, nx] = dist + current.depth;
                    temp = new CellInfo(maze[ny, nx], ny, nx, current.depth + 1, dist);
                    temp.parent = current;
                    pq.Enqueue(temp);
                }
            }
            // move front
            if (!current.cell.WallFront && !maze[current.y + 1, current.x].WallBack) 
            {
                ny = current.y + 1;
                nx = current.x;
                dist = GetManhattanDist(ny, nx, targetY, targetX);
                if (evals[ny, nx] > dist + current.depth + 1)
                {
                    evals[ny, nx] = dist + current.depth;
                    temp = new CellInfo(maze[ny, nx], ny, nx, current.depth + 1, dist);
                    temp.parent = current;
                    pq.Enqueue(temp);
                }
            }
            // move left
            if (!current.cell.WallLeft && !maze[current.y, current.x - 1].WallRight) 
            {
                ny = current.y;
                nx = current.x - 1;
                dist = GetManhattanDist(ny, nx, targetY, targetX);
                if (evals[ny, nx] > dist + current.depth + 1)
                {
                    evals[ny, nx] = dist + current.depth;
                    temp = new CellInfo(maze[ny, nx], ny, nx, current.depth + 1, dist);
                    temp.parent = current;
                    pq.Enqueue(temp);
                }
            }
            // move back
            if (!current.cell.WallBack && !maze[current.y - 1, current.x].WallFront)
            {
                ny = current.y - 1;
                nx = current.x;
                dist = GetManhattanDist(ny, nx, targetY, targetX);
                if (evals[ny, nx] > dist + current.depth + 1)
                {
                    evals[ny, nx] = dist + current.depth;
                    temp = new CellInfo(maze[ny, nx], ny, nx, current.depth + 1, dist);
                    temp.parent = current;
                    pq.Enqueue(temp);
                }
            }
        }

        yield break;
    }
    private int GetManhattanDist(int y, int x, int targetY, int targetX)
    {
        /*int exitY = rows - 1;
        int exitX = columns - 1;*/
        int manhattanDist = Mathf.Abs(y - targetY) + Mathf.Abs(x - targetX);

        return manhattanDist;
    }

}
