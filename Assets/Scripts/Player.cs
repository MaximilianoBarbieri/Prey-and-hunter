using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]private float _speed = 3f;
    [SerializeField] private float _findRadius = 0.7f; //radio justo para que no toque otros
    [SerializeField] private Node _lastNode;
    
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

        RefreshLastNode();
    }

    private void MovePlayer(Vector3 movement)
    {
        Vector3 targetPosition = transform.position + movement * (_speed * Time.deltaTime);

        Collider[] colliders = Physics.OverlapSphere(targetPosition, 0.25f, wallLayer);
        if (colliders.Length == 0)
        {
            transform.position = targetPosition;
        }
    }

    private void RefreshLastNode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _findRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Node")) 
                _lastNode = collider.transform.GetComponent<Node>();
        }
    }

    public Node GetLastNode()
    {
        return _lastNode;
    }
    
    public void ChangeColor(Color color) => _rend.material.color = color; //TODO: ver si se usar este metodo
    
    //Debug Radius//
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _findRadius);
    }
}
