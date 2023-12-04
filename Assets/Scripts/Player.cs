using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]private float _speed = 3f;
    public Color myInitialMaterialColor;
    [SerializeField] Renderer _rend;

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
    
    public void ChangeColor(Color color) => _rend.material.color = color;
}
