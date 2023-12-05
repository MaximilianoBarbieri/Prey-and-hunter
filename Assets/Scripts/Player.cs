using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PathFinding _pf = new PathFinding();
    List<Vector3> _path = new List<Vector3>();

    [SerializeField] float _speed = 2.5f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            GoToStartingNode();
            _path = GetPathBase();
            if (_path?.Count > 0) _path.Reverse();
        }

        if (_path.Count > 0 && Input.GetKey(KeyCode.O)) TravelPath();
    }

    private void GoToStartingNode()
    {
        if (PathFindingManager.instance.GetStartingNode() == null) return;
        transform.position = PathFindingManager.instance.GetStartingNode().transform.position;
    }

    private void TravelPath()
    {
        Vector3 target = _path[0];
        Vector3 dir = target - transform.position;
        transform.position += dir.normalized * (_speed * Time.deltaTime);

        if (Vector3.Distance(target, transform.position) <= 0.1f) _path.RemoveAt(0);
    }
    
    List<Vector3> GetPathBase()
    {
        switch (PathFindingManager.instance.pfType)
        {
            case PathfindingType.AStar:
                return _pf.AStar(PathFindingManager.instance.GetStartingNode(), PathFindingManager.instance.GetGoalNode());
        }

        return new List<Vector3>();
    }
    
    //TODO: Adaptarlo a lo actual de ser necesario
    /*[Header("Properties")]
    [SerializeField]private float _speed = 3f;
    public Color myInitialMaterialColor;
    Renderer _rend;

    public LayerMask wallLayer; // Layer para las paredes

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        myInitialMaterialColor = _rend.material.color;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        MovePlayer(movement);
    }

    private void MovePlayer(Vector3 movement)
    {
        Vector3 targetPosition = transform.position + movement * (_speed * Time.deltaTime);

        Collider[] colliders = Physics.OverlapSphere(targetPosition, 0.2f, wallLayer);
        if (colliders.Length == 0)
        {
            transform.position = targetPosition;
        }
    }
    
    public void ChangeColor(Color color) => _rend.material.color = color;*/
}
