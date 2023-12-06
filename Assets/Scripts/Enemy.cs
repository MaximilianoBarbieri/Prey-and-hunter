using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    //PathFinding//
    private PathFinding _pf = new();
    private List<Vector3> _path = new();
    public List<Vector3> Path => _path;
    private int _currentNodeIndex = 0;

    [Header("PATROL")] [SerializeField] public List<Node> patrolNodes = new();
    [SerializeField] public Node _currentNode;
    [SerializeField] public int _currentNodePatrol = 0;
    [SerializeField] private float _velocity = 2f;

    [Header("FOV")] [SerializeField] private Player _player; //cuando ve al player
    [SerializeField] LayerMask _obstacle; //layer que interrumpe su vista

    [SerializeField, Range(1, 10)] float _viewRadius;
    [SerializeField, Range(1, 360)] float _viewAngle;

    StateMachine _sm;


    private void Awake()
    {
        transform.position = patrolNodes[_currentNodePatrol].transform.position; // ubico al enemy en el nodo 0
        _currentNode = patrolNodes[0]; //Le asigno el nodo 0 de su patrullaje
    }

    private void Start()
    {
        _sm = new StateMachine();

        _sm.AddState(EnemyState.Patrol, new Patrol(this));
        _sm.AddState(EnemyState.Chase, new Chase(this, _player));
        _sm.AddState(EnemyState.Hunt, new Hunt(this));
        _sm.AddState(EnemyState.ReturnToPatrol, new ReturnToPatrol(this));

        _sm.ChangeState(EnemyState.Patrol);
    }

    private void Update()
    {
        CheckCurrentNode();
        
        _sm.Update();
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            _sm.ChangeState(EnemyState.Hunt);
        }
        
    }

    public void MoveTo(Vector3 dir)
    {
        dir -= transform.position;
        dir = dir.normalized;
        transform.forward = dir;
        transform.position += transform.forward * (Time.deltaTime * _velocity);
    }

    public bool InFieldOfView()
    {
        Vector3 dir = _player.transform.position - transform.position;
        if (dir.magnitude > _viewRadius) return false;
        if (!InLineOfSight(transform.position, _player.transform.position)) return false;
        if (Vector3.Angle(transform.forward, dir) > _viewAngle / 2) return false;
        return true;
    }

    bool InLineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, _obstacle);
    }

    Vector3 GetAngleFromDir(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
    // Método para cazar (deberá usar A* para ir del nodo actual hacia el último nodo donde estuvo el player)
    public void Hunt() //Funciona
    {
        _path = _pf.AStar(_currentNode, _player.GetLastNode());
        if (_path?.Count > 0)
        {
            if (Vector3.Distance(_currentNode.transform.position, transform.position) >= 0.5f) //si estoy lejos vuelvo al nodo, evito traspasar paredes
                _path.Add(_currentNode.transform.position);
            _path.Reverse();
            _currentNodeIndex = 0;
        }
    }

    public void TravelPath()
    {
        Vector3 target = _path[0];
        MoveTo(target);

        if (Vector3.Distance(target, transform.position) <= 0.1f) _path.RemoveAt(0);
    }
    
    public void ReturnToPatrol() 
    {
        _path = _pf.AStar(_currentNode, patrolNodes[0]); //desde el ult nodo que toque hasta el 1ero del patrullaje
        if (_path?.Count > 0)
        {
            if (Vector3.Distance(_currentNode.transform.position, transform.position) >= 0.5f) //si estoy lejos vuelvo al nodo, evito traspasar paredes
                _path.Add(_currentNode.transform.position);
            _path.Reverse();
            _currentNodeIndex = 0;
        }
    }


    public void PatrolAStar() //Patrullaje de la lista en bucle
    {
        if (_currentNodePatrol < patrolNodes.Count - 1 &&
            _path.Count == 0) // Generar el nuevo camino si el camino actual está vacío
            _path = _pf.AStar(patrolNodes[_currentNodePatrol], patrolNodes[_currentNodePatrol + 1]);

        // Verificar si el camino actual no está vacío
        if (_path.Count > 0)
        {
            Vector3 target = _path[0];
            MoveTo(target);

            // Verificar si hemos llegado al nodo de destino
            if (Vector3.Distance(target, transform.position) <= 0.1f)
            {
                _path.RemoveAt(0);

                // Verificar si llegamos al último nodo en la patrulla
                if (_path.Count == 0)
                {
                    _currentNodePatrol++;
                    // Si llegamos al último nodo, volver al primero para repetir en bucle
                    if (_currentNodePatrol == patrolNodes.Count)
                        _currentNodePatrol = 0;

                    // Generar el nuevo camino si hay nodos restantes en la lista
                    if (_currentNodePatrol < patrolNodes.Count - 1)
                        _path = _pf.AStar(patrolNodes[_currentNodePatrol], patrolNodes[_currentNodePatrol + 1]);
                    else if
                        (_currentNodePatrol == patrolNodes.Count - 1) // Si llegamos al último nodo, volver al primero para repetir en bucle
                        _path = _pf.AStar(patrolNodes[_currentNodePatrol], patrolNodes[0]);
                }
            }
        }
    }


    private void CheckCurrentNode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Node"))
                _currentNode = collider.transform.GetComponent<Node>();
        }
    }

    //Debug Radius//
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Vector3 DirA = GetAngleFromDir(_viewAngle / 2 + transform.eulerAngles.y);
        Vector3 DirB = GetAngleFromDir(-_viewAngle / 2 + transform.eulerAngles.y);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + DirA.normalized * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + DirB.normalized * _viewRadius);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}

public enum EnemyState
{
    Patrol,
    ReturnToPatrol,
    Chase,
    Hunt
}