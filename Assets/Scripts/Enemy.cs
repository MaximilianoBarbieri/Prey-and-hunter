using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("PATROL")] [SerializeField] private List<Node> patrolNodes = new(); // Lista de nodos predeterminados
    private int _currentNodePatrol = 0;
    public float velocidadMovimiento = 2f;

    [Header("FOV")]
    [SerializeField] private Player _player; //cuando ve al player
    [SerializeField]
    LayerMask _obstacle; //layer que interrumpe su vista

    [SerializeField, Range(1, 10)] float _viewRadius;
    [SerializeField, Range(1, 360)] float _viewAngle;

    void Start()
    {
        //StartCoroutine(CheckFOVRepeatedly()); //Check de FOV en segundo plano
    }

    private void FixedUpdate()
    {
        Patrullar();
        CheckFOV(); //Check de FOV en segundo plano
    }

    void Patrullar() //TODO: Agregarlo a FSM
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolNodes[_currentNodePatrol].transform.position,
            velocidadMovimiento * Time.deltaTime);

        if (Vector3.Distance(transform.position, patrolNodes[_currentNodePatrol].transform.position) < 0.1f)
        {
            _currentNodePatrol = (_currentNodePatrol + 1) % patrolNodes.Count; //Siguiente nodo de la lista
        }
    }

    // Codigo Para chequear el FOV
    IEnumerator CheckFOVRepeatedly()
    {
        while (true)
        {
            CheckFOV();
            yield return null; // Espera un frame antes de la siguiente verificación
        }
    }
    
    void CheckFOV()
    {
        _player.ChangeColor(InFieldOfView(_player.transform.position) ? Color.blue : _player.myInitialMaterialColor);
    }

    bool InFieldOfView(Vector3 endPos)
    {
        Vector3 dir = endPos - transform.position;
        if (dir.magnitude > _viewRadius) return false;
        if (!InLineOfSight(transform.position, endPos)) return false;
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
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Vector3 DirA = GetAngleFromDir(_viewAngle / 2);
        Vector3 DirB = GetAngleFromDir(-_viewAngle / 2);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + DirA.normalized * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + DirB.normalized * _viewRadius);
    }
    
    
    /*void CheckFOV()
    {
        _player.ChangeColor(InFieldOfView(_player.transform.position) ? Color.blue : _player.myInitialMaterialColor);
    }

    bool InFieldOfView(Vector3 endPos)
    {
        Vector3 dir = endPos - transform.position;
        if (dir.magnitude > _viewRadius) return false;
        if (!InLineOfSight(transform.position, endPos)) return false;
        if (Vector3.Angle(transform.forward, dir) > _viewAngle / 2) return false;
        return true;
    }

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
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }*/
}