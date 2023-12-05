using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("PATROL")] 
    [SerializeField] public List<Node> patrolNodes = new();
    [SerializeField] public int _currentNodePatrol = 0;
    [SerializeField] public Node _currentNode; //Se crea esta var para saber en que nodo se encuentra si sale de patrol
    [SerializeField] private float _velocity = 2f;
    
    //PathFinding//
    private PathFinding _pf = new();
    private List<Vector3> _path = new();

    [Header("FOV")] 
    [SerializeField] private Player _player; //cuando ve al player
    [SerializeField] LayerMask _obstacle; //layer que interrumpe su vista

    [SerializeField, Range(1, 10)] float _viewRadius;
    [SerializeField, Range(1, 360)] float _viewAngle;

    StateMachine _sm;


    private void Awake()
    {
        transform.position = patrolNodes[_currentNodePatrol].transform.position;
    }

    private void Start()
    {
        _sm = new StateMachine();

        _sm.AddState(EnemyState.Patrol, new Patrol(this));
        _sm.AddState(EnemyState.Chase, new Chase(this, _player));
        _sm.AddState(EnemyState.Hunt, new Hunt(this));
        _sm.AddState(EnemyState.ReturnToPatrol, new ReturnToPatrol(this));

        _sm.ChangeState(EnemyState.Patrol); //Comienzo con el state Patrol
    }

    private void Update()
    {
        CheckCurrentNode();
        _sm.Update();
    }

    public void MoveTo(Vector3 dir)
    {
        dir -= transform.position;
        dir = dir.normalized;
        transform.forward = dir;
        transform.position += transform.forward * (Time.deltaTime * _velocity);
    }

    //FOV (Field of View)
    public bool InFieldOfView()
    {
        Vector3 dir = _player.transform.position - transform.position;
        if (dir.magnitude > _viewRadius) return false;
        if (!InLineOfSight(transform.position, _player.transform.position)) return false;
        if (Vector3.Angle(transform.forward, dir) > _viewAngle / 2) return false;
        return true;
    }

    //LOS (Line of Sight)
    bool InLineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, _obstacle);
    }

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

    Vector3 GetAngleFromDir(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    ////////////////////////////////////////////////////////////////
    
    public void PatrolAStar() //Patrullaje de la lista en bucle
    {
        if (_currentNodePatrol < patrolNodes.Count - 1 && _path.Count == 0) // Generar el nuevo camino si el camino actual está vacío
            _path = _pf.AStar(patrolNodes[_currentNodePatrol], patrolNodes[_currentNodePatrol + 1]);

        // Verificar si el camino actual no está vacío
        if (_path.Count > 0)
        {
            Vector3 target = _path[0];
            //Vector3 dir = target - transform.position;
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
                    else if (_currentNodePatrol == patrolNodes.Count - 1)                        // Si llegamos al último nodo, volver al primero para repetir en bucle
                        _path = _pf.AStar(patrolNodes[_currentNodePatrol], patrolNodes[0]);
                }
            }
        }
    }
    
    public void ChasePlayer() //Cazar al jugador sabiendo el ultimo nodo donde se encontro
    {
        Debug.Log("Estoy en chaseplayer metod");
        if (_player.GetLastNode() != null)
        {
            _path = _pf.AStar(_currentNode, _player.GetLastNode());
            if (_path?.Count > 0)
            {
                _path.Reverse();
                TravelPath();
            }
        }
    }
    
    private void TravelPath()
    {
        Vector3 target = _path[0] - Vector3.forward;
        Vector3 dir = target - transform.position;
        transform.position += dir.normalized * _velocity * Time.deltaTime;

        if (Vector3.Distance(target, transform.position) <= 0.1f) _path.RemoveAt(0);
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