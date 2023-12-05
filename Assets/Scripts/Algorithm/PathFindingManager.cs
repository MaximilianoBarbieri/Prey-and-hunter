using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//Es opcional hacerlo, este script va a mostrar el pathfinding en este proyecto.
public class PathFindingManager : MonoBehaviour
{
    public static PathFindingManager instance;

    public PathfindingType pfType;

    Dictionary<PathfindingType, Func<Node, Node, IEnumerator>> _pfCoroutine;

    [SerializeField] Node _startingNode, _goalNode;
    public Color _startingNodeColor;
    public Color _goalNodeColor;
    public Color _previousNodeColor;

    PathFinding _pf = new();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        _pfCoroutine = new Dictionary<PathfindingType, Func<Node, Node, IEnumerator>>();
        _pfCoroutine.Add(PathfindingType.AStar, _pf.AStarCoroutine);
    }

    public void SetMyStartingNode(Node node) //Lo voy a necesitar para setear el nodo donde se empieza
    {
        if (_startingNode != null) _startingNode.NewColor(_previousNodeColor);
        _startingNode?.NewColor(_previousNodeColor);
        _startingNode = node;
        _previousNodeColor = node.PreviousColor();
        node.NewColor(_startingNodeColor);
    }

    public void SetMyGoalNode(Node node)
    {
        _goalNode?.NewColor(_previousNodeColor);
        _goalNode = node;
        _previousNodeColor = node.PreviousColor();
        node.NewColor(_goalNodeColor);
    }


    void Update()
    {
        if (_startingNode == null || _goalNode == null) return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _startingNode.PreviousColor();
            _goalNode.PreviousColor();
            StartCoroutine(_pfCoroutine[0](_startingNode, _goalNode));
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public Node GetStartingNode() => _startingNode;

    public Node GetGoalNode() => _goalNode;
}

public enum PathfindingType
{
    AStar
}