using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("PATROL")] [SerializeField] public List<Node> patrolNodes = new(); // Lista de nodos predeterminados
    public int _currentNodePatrol = 0;
    [SerializeField] private float _velocity = 2f;

    [Header("FOV")] [SerializeField] private Player _player; //cuando ve al player
    [SerializeField] LayerMask _obstacle; //layer que interrumpe su vista

    [SerializeField, Range(1, 10)] float _viewRadius;
    [SerializeField, Range(1, 360)] float _viewAngle;

    StateMachine _sm;

    private void Start()
    {
        _sm = new StateMachine(); //Esto esta creando una nueva FSM;
        
        _sm.AddState(EnemyState.Patrol, new Patrol(this));
        _sm.AddState(EnemyState.Chase, new Chase(this, _player));
        _sm.AddState(EnemyState.Hunt, new Hunt(this));
        _sm.AddState(EnemyState.ReturnToPatrol, new ReturnToPatrol(this));
        
        _sm.ChangeState(EnemyState.Patrol);
    }

    private void Update()
    {
        //CheckFOV();
        _sm.Update();
    }

    private void FixedUpdate()
    {
        //Patrullar();
    }

    public void MoveTo(Vector3 dir)
    {
        dir -= transform.position;
        dir = dir.normalized;
        transform.forward = dir;
        transform.position += transform.forward * (Time.deltaTime * _velocity);
    }

    public void Patrullar() //TODO: Agregarlo a FSM
    {
        /*transform.position = Vector3.MoveTowards(transform.position, patrolNodes[_currentNodePatrol].transform.position,
            velocidadMovimiento * Time.deltaTime);*/
        MoveTo(patrolNodes[_currentNodePatrol].transform.position);

        if (Vector3.Distance(transform.position, patrolNodes[_currentNodePatrol].transform.position) < 0.01f)
        {
            _currentNodePatrol = (_currentNodePatrol + 1) % patrolNodes.Count; //Siguiente nodo de la lista
        }
    }

    void CheckFOV()
    {
        //_player.ChangeColor(InFieldOfView(_player.transform.position) ? Color.blue : _player.myInitialMaterialColor);
        //Debug.Log("FOV : " + InFieldOfView(_player.transform.position));
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
}

public enum EnemyState
{
    Patrol,
    ReturnToPatrol,
    Chase,
    Hunt
}