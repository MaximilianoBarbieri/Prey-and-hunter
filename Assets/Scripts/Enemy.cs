using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("PATROL")] 
    [SerializeField] public List<Node> patrolNodes = new(); // Lista de nodos predeterminados
    [SerializeField] public int _currentNodePatrol = 0;
    [SerializeField] private float _velocity = 2f;
    
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
    
    public void PatrolAStar()
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
                    Debug.Log("ANTES "+ _currentNodePatrol);
                    _currentNodePatrol++;
                    Debug.Log("ME SUME "+ _currentNodePatrol);
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
    
}

public enum EnemyState
{
    Patrol,
    ReturnToPatrol,
    Chase,
    Hunt
}