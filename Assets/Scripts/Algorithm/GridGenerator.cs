using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] int _sizeX,_sizeY;
    [SerializeField] GameObject _nodePrefab;
    [SerializeField] float _offSet = 0.1f;
    Node[,] _grid;

    /*void Start()
    {
        StartCoroutine(GenerateGrid());
    }

    IEnumerator GenerateGrid()
    {
        _grid = new Node[_sizeX, _sizeY];

        WaitForSeconds time = new WaitForSeconds(0.01f);

        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                GameObject cube = Instantiate(_nodePrefab);

                cube.transform.position = Vector3.right * (x + (_offSet * x)) +
                    Vector3.up * (y + (_offSet * y));

                cube.transform.SetParent(transform);

                Node node = cube.GetComponent<Node>();

                node.Initialize(this, x, y);

                _grid[x, y] = node;

                yield return time;
            }
        }
    }

    //Conseguir vecinos
    public List<Node> GetNeighborsAtPosition(int x, int y)
    {
        List<Node> neighbors = new List<Node>();

        if (x + 1 < _sizeX) neighbors.Add(_grid[x + 1, y]); 
        //Si el valor X actual sigue siendo menor al tamaño de la grilla lo agregamos

        if(y - 1 >= 0) neighbors.Add(_grid[x, y - 1]); //Si el valor Y actual - 1 sigue siendo positivo o 0 lo agregamos

        if (x - 1 >= 0) neighbors.Add(_grid[x - 1, y]);//Si el valor X actual - 1 sigue siendo positivo o 0 lo agregamos

        if(y + 1 < _sizeY) neighbors.Add(_grid[x, y + 1]);

        return neighbors;
    }*/

    public void ResetColors(Node start, Node goal)
    {
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                Node node = _grid[x, y];
                if (node == start || node == goal) continue;
                node.NewColor(new Color(1, 0.711714f,0));
            }
        }
    }

}
