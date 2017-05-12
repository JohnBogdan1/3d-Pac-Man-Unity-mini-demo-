using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{

    private static int MAXINT = 0x7FFFFFFF;
    public int _width, _height; // maze sizes

    public VisualCell visualCellPrefab; // the actual cell

    public Cell[,] cells; // maze cells using a matrix

    private Vector2 _randomCellPos; // cell position that start the generation
    private VisualCell visualCell; // it is a copy of the prefab

    private List<CellAndRelativePosition> neighbors;
    private List<GameObject> foodList;

    public GameObject pacman, enemy1, enemy2, enemy3, enemy4, food;

    public static int[] line = { -1, 0, 1, 0 };
    public static int[] column = { 0, 1, 0, -1 };

    public static Vector3[] randPos;

    public string toMainMenu = "";

    public bool isPaused = false;

    public GameObject pauseMenu = null;

    void Start()
    {
        cells = new Cell[_width, _height]; // initialize the table
        Init();
        randPos = new Vector3[5];
        randPos[0] = new Vector3(Random.Range(0, _width) * 3, 0, _height * 3 - Random.Range(0, _height) * 3);
        pacman = Instantiate(pacman, randPos[0], Quaternion.identity) as GameObject;

        randPos[1] = new Vector3(Random.Range(0, _width) * 3, 0, _height * 3 - Random.Range(0, _height) * 3);
        enemy1 = Instantiate(enemy1, randPos[1], Quaternion.identity) as GameObject;

        /*randPos[2] = new Vector3(Random.Range(0, _width) * 3, 0, _height * 3 - Random.Range(0, _height) * 3);
        enemy2 = Instantiate(enemy2, randPos[2], Quaternion.identity) as GameObject;

        randPos[3] = new Vector3(Random.Range(0, _width) * 3, 0, _height * 3 - Random.Range(0, _height) * 3);
        enemy3 = Instantiate(enemy3, randPos[3], Quaternion.identity) as GameObject;

        randPos[4] = new Vector3(Random.Range(0, _width) * 3, 0, _height * 3 - Random.Range(0, _height) * 3);
        enemy4 = Instantiate(enemy4, randPos[4], Quaternion.identity) as GameObject;*/

        Vector3 foodPos;

        foodList = new List<GameObject>();

        for (int i = 0; i < _width; ++i)
        {
            for (int j = 0; j < _height; ++j)
            {
                foodPos = new Vector3(i * 3, 0, _height * 3 - j * 3);

                if (!IsOnCharacters(foodPos, randPos))
                {
                    food = Instantiate(food, foodPos, Quaternion.identity) as GameObject;
                    foodList.Add(food);
                }

            }
        }


        // call pathfinding
        Vector3 enemy = new Vector3(randPos[1].x / 3, 0, ((_height * 3 - randPos[1].z) / 3));
        Vector3 player = new Vector3(randPos[0].x / 3, 0, ((_height * 3 - randPos[0].z) / 3));
        PathFinding pf = new PathFinding(enemy, player, _width, _height, cells);
        //Debug.Log("Inamic 1 la pozitia: " + (randPos[1].x / 3) + " " + ((_height * 3 - randPos[1].z) / 3));
        //Debug.Log(pf.GetDirection());

        /*enemy = new Vector3(randPos[2].x / 3, 0, ((_height * 3 - randPos[2].z) / 3));
        player = new Vector3(randPos[0].x / 3, 0, ((_height * 3 - randPos[0].z) / 3));
        pf = new PathFinding(enemy, player, _width, _height, cells);
        Debug.Log("Inamic 2 la pozitia: " + (randPos[2].x / 3) + " " + ((_height * 3 - randPos[2].z) / 3));
        Debug.Log(pf.GetDirection());

        enemy = new Vector3(randPos[3].x / 3, 0, ((_height * 3 - randPos[3].z) / 3));
        player = new Vector3(randPos[0].x / 3, 0, ((_height * 3 - randPos[0].z) / 3));
        pf = new PathFinding(enemy, player, _width, _height, cells);
        Debug.Log("Inamic 3 la pozitia: " + (randPos[3].x / 3) + " " + ((_height * 3 - randPos[3].z) / 3));
        Debug.Log(pf.GetDirection());

        enemy = new Vector3(randPos[4].x / 3, 0, ((_height * 3 - randPos[4].z) / 3));
        player = new Vector3(randPos[0].x / 3, 0, ((_height * 3 - randPos[0].z) / 3));
        pf = new PathFinding(enemy, player, _width, _height, cells);
        Debug.Log("Inamic 4 la pozitia: " + (randPos[4].x / 3) + " " + ((_height * 3 - randPos[4].z) / 3));
        Debug.Log(pf.GetDirection());*/
    }

    public class PathFinding
    {
        Vector3 enemyPosition;
        Vector3 playerPosition;

        private int _width, _height; // maze sizes
        private bool[,] visited;

        private Cell[,] cells;

        public PathFinding(Vector3 enemyPosition, Vector3 playerPosition, int _width, int _height, Cell[,] cells)
        {
            this.enemyPosition = enemyPosition;
            this.playerPosition = playerPosition;
            this._width = _width;
            this._height = _height;
            this.cells = cells;
        }

        public int GetDirection()
        {

            int direction = -1, min = MAXINT;

            /*for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                    if (i == cells[i, j].xPos && j == cells[i, j].zPos)
                        Debug.Log("true");*/
            for (int i = 0; i < 4; i++)
            {
                Vector3 v = new Vector3((int)enemyPosition.x + column[i], 0, (int)enemyPosition.z + line[i]);
                //Debug.Log("---------------> " + i + "\n");
                //Vector3 v = enemyPosition;
                int distance;
                if (IsWall(enemyPosition, i))
                {
                    distance = MAXINT;
                    //Debug.Log("\ndirection=:" + i + "   distance" + distance + " by IsWall \n");
                }
                else
                {
                    //distance = myDftIterative(enemyPosition);
                    distance = bfs_get_direction(v);
                    //Debug.Log("\ndirection=:" + i + "   distance" + distance + " by bfs \n");
                }
                if (distance < min)
                {
                    min = distance;
                    direction = i;
                }
                
            }
            return direction;
        }


        public int myDftIterative(Vector3 srcVertex)
        {

            bool[,] visited = new bool[_width, _height];
            int[,] distance = new int[_width, _height];
            int finishX = (int)playerPosition.x;
            int finishY = (int)playerPosition.z;

            Stack<Vector3> vertexStack = new Stack<Vector3>();
            vertexStack.Push(srcVertex);

            while (vertexStack.Count > 0)
            {
                Vector3 vertex = vertexStack.Pop();


                if (visited[(int)vertex.x, (int)vertex.z])
                    continue;

                //Console.Write(vertex.x + " " + vertex.y + "\n");
                visited[(int)vertex.x, (int)vertex.z] = true;


                for (int neighbour = 0; neighbour < 4; neighbour++)
                {
                    int newl = (int)vertex.x + line[neighbour];
                    int newc = (int)vertex.z + column[neighbour];
                    int newd = distance[(int)vertex.x, (int)vertex.z] + 1;

                    if (newl < 0) newl = _width - 1;
                    else if (newl >= _width) newl = 0;
                    if (newc < 0) newc = _height - 1;
                    else if (newc >= _height) newc = 0;

                    if (visited[newl, newc] == true)
                    {
                        distance[newl, newc] = System.Math.Min(newd, distance[newl, newc]);
                    }
                    else
                    {
                        distance[newl, newc] = newd;
                    }

                    if (newl == finishX && newc == finishY)
                    {
                        return distance[newl, newc];
                    }
                    Vector3 new_vertex = new Vector3(newl, 0, newc);

                    if (!IsWall(vertex, neighbour) && !visited[(int)new_vertex.x, (int)new_vertex.z])
                    {
                        Debug.Log("Direction: " + neighbour + " New Pos: " + vertex);
                        vertexStack.Push(new_vertex);
                    }
                }
            }
            return MAXINT;
        }

        public int bfs_get_direction(Vector3 vertex)
        {
            int[] my_column = { -1, 0, 1, 0 };
            int[] my_line = { 0, 1, 0, -1 };
            if (vertex.x == playerPosition.x && vertex.z == playerPosition.z)
            {
                return 0;
            }
            Queue<Vector3> q = new Queue<Vector3>();
            bool[,] visited = new bool[_width, _height];
            int[,] level = new int[_width, _height];
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                {
                    visited[i, j] = false;
                    level[i, j] = -1;
                }

            q.Enqueue(vertex);
            visited[(int)vertex.x, (int)vertex.z] = true;
            level[(int)vertex.x, (int)vertex.z] = 0;
            while (q.Count > 0)
            {
                Vector3 crt = q.Dequeue();

                for (int i = 0; i < 4; i++)
                {
                    Vector3 copil = new Vector3((int)crt.x + my_line[i], 0, (int)crt.z + my_column[i]);
                    

                    if (copil.x < 0 || copil.x >= _width || copil.z < 0 || copil.z >= _height ||
                        visited[(int)copil.x, (int)copil.z] == true || IsWall(crt, i))
                    {

                    }
                    else
                    {
                        if (copil.x == playerPosition.x && copil.z == playerPosition.z)
                        {
                            return level[(int)crt.x, (int)crt.z] + 1;
                        }
                        //Debug.Log("Parinte: " + crt.x + " " + crt.z);
                        //Debug.Log("Copil: " + copil.x + " " + copil.z);
                        visited[(int)copil.x, (int)copil.z] = true;
                        level[(int)copil.x, (int)copil.z] = level[(int)crt.x, (int)crt.z] + 1;
                        q.Enqueue(copil);
                    }
                }

            }
            return MAXINT;
        }

        private bool IsWall(Vector3 v, int dir)
        {
            //Debug.Log("Direction: " + dir);
            //Debug.Log("x: " + (int)v.x + "z: " + (int)v.z);
            if ((int)v.x < 0 || (int)v.z >= _height || (int)v.x >= _width || (int)v.z < 0)
            {
                return true;
            }

            //Debug.Log("C_East: " + cells[(int)(v.x), (int)v.z]._East);
            //Debug.Log("C_West: " + cells[(int)(v.x), (int)v.z]._West);
            //Debug.Log("C_North: " + cells[(int)(v.x), (int)v.z]._North);
            //Debug.Log("C_South: " + cells[(int)(v.x), (int)v.z]._South);
            switch (dir)
            {
                case (0):
                    //Debug.Log("0: " + (int)(v.x) + " " + (int)(v.z) + " " + cells[(int)(v.x), (int)v.z]._South);
                    if (!cells[(int)(v.x), (int)v.z]._North)
                    {
                        return true;
                    }
                    break;
                case (1):
                    //Debug.Log("1: " + (int)(v.x) + " " + (int)(v.z) + " " + cells[(int)(v.x), (int)v.z]._West);
                    if (!cells[(int)v.x, (int)(v.z)]._East)
                    {
                        return true;
                    }
                    break;
                case (2):
                    //Debug.Log("2: " + (int)(v.x) + " " + (int)(v.z) + " " + cells[(int)(v.x), (int)v.z]._North);
                    if (!cells[(int)(v.x), (int)v.z]._South)
                    {
                        return true;
                    }
                    break;
                case (3):
                    //Debug.Log("3: " + (int)(v.x) + " " + (int)(v.z) + " " + cells[(int)(v.x), (int)v.z]._East);
                    if (!cells[(int)v.x, (int)(v.z)]._West)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
    }

    bool IsOnCharacters(Vector3 position, Vector3[] positions)
    {

        for (int i = 0; i < 4; i++)
        {
            if (position.x == positions[i].x && position.z == positions[i].z)
            {
                return true;
            }
        }
        return false;
    }

    void Init()
    {
        for (int i = 0; i < _width; ++i)
        {
            for (int j = 0; j < _height; ++j)
            {
                cells[i, j] = new Cell(false, false, false, false, false); // initialize a cell
                cells[i, j].xPos = i;
                cells[i, j].zPos = j;
            }
        }
        _randomCellPos = new Vector2(Random.Range(0, _width), Random.Range(0, _height)); // select a cell where to start from

        //Debug.Log(_randomCellPos.x + " " + _randomCellPos.y);
        GenerateMaze((int)_randomCellPos.x, (int)_randomCellPos.y); // generate maze starting from that cell

        DrawCells(); // draw all the remaining cells
    }

    // generate maze using DFS
    void GenerateMaze(int x, int y)
    {
        Cell currentCell = cells[x, y]; // current cell
        neighbors = new List<CellAndRelativePosition>();
        if (currentCell._visited == true) return;
        currentCell._visited = true;

        // check if one of the neighbors(up, left, right, down) was visited; if not, add it to list  
        if (x + 1 < _width && cells[x + 1, y]._visited == false)
        {
            neighbors.Add(new CellAndRelativePosition(cells[x + 1, y], CellAndRelativePosition.Direction.East));
        }

        if (y + 1 < _height && cells[x, y + 1]._visited == false)
        {
            neighbors.Add(new CellAndRelativePosition(cells[x, y + 1], CellAndRelativePosition.Direction.South));
        }

        if (x - 1 >= 0 && cells[x - 1, y]._visited == false)
        {
            neighbors.Add(new CellAndRelativePosition(cells[x - 1, y], CellAndRelativePosition.Direction.West));
        }

        if (y - 1 >= 0 && cells[x, y - 1]._visited == false)
        {
            neighbors.Add(new CellAndRelativePosition(cells[x, y - 1], CellAndRelativePosition.Direction.North));
        }

        if (neighbors.Count == 0) return;

        neighbors.Shuffle();

        // for each neighbor
        foreach (CellAndRelativePosition selectedcell in neighbors)
        {
            if (selectedcell.direction == CellAndRelativePosition.Direction.East)
            {
                if (selectedcell.cell._visited) continue;
                currentCell._East = true; // delete the right wall of the current cell
                selectedcell.cell._West = true; // delete the left wall of the neighbor selected
                GenerateMaze(x + 1, y); // start from the neighbor
            }

            else if (selectedcell.direction == CellAndRelativePosition.Direction.South)
            { // En bas
                if (selectedcell.cell._visited) continue;
                currentCell._South = true; // delete the behind wall of the current cell
                selectedcell.cell._North = true; // delete the above wall of the neighbor selected
                GenerateMaze(x, y + 1); // start from the neighbor
            }
            else if (selectedcell.direction == CellAndRelativePosition.Direction.West)
            { // A gauche
                if (selectedcell.cell._visited) continue;
                currentCell._West = true; // delete the left wall of the current cell
                selectedcell.cell._East = true; // delete the right wall of the neighbor selected
                GenerateMaze(x - 1, y); // start from the neighbor
            }
            else if (selectedcell.direction == CellAndRelativePosition.Direction.North)
            { // En haut
                if (selectedcell.cell._visited) continue;
                currentCell._North = true; // delete the above wall of the current cell
                selectedcell.cell._South = true; // delete the behind wall of the neighbor selected
                GenerateMaze(x, y - 1); // start from the neighbor
            }
        }


    }

    void DrawCells()
    {
        // draw the cells of the maze
        foreach (Cell cell in cells)
        {
            Vector3 pos = new Vector3(cell.xPos * 3, 0, _height * 3 - cell.zPos * 3);
            visualCell = Instantiate(visualCellPrefab, pos, Quaternion.identity) as VisualCell;
            visualCell.transform.parent = transform;
            // if north, east, south or west are made true in the MazeGenerator, then delete that wall
            visualCell._North.gameObject.SetActive(!cell._North);
            visualCell._South.gameObject.SetActive(!cell._South);
            visualCell._Est.gameObject.SetActive(!cell._East);
            visualCell._West.gameObject.SetActive(!cell._West);

            visualCell.transform.name = "[ " + cell.xPos.ToString() + ", " + cell.zPos.ToString() + " ]";
        }

    }

    //update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            Debug.Log("PAUSED");
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Debug.Log("UNPAUSED");
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            isPaused = !isPaused;
        }
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void Quit()
    {
        Application.LoadLevel(toMainMenu);
    }
}
