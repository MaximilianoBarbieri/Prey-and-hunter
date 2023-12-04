using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    public float speed = 3f; // Velocidad de movimiento del jugador
    Renderer _rend;
    public Color myInitialMaterialColor;
    
    public LayerMask wallLayer; // Layer para las paredes

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        myInitialMaterialColor = _rend.material.color;
    }

    private void Update()
    {
        // Obtener entrada del teclado
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calcular la dirección del movimiento
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Mover al jugador
        MovePlayer(movement);
    }

    private void MovePlayer(Vector3 movement)
    {
        // Calcular la posición objetivo
        Vector3 targetPosition = transform.position + movement * (speed * Time.deltaTime);

        // Verificar si hay colisión con las paredes
        Collider[] colliders = Physics.OverlapSphere(targetPosition, 0.2f, wallLayer);
        if (colliders.Length == 0)
        {
            // Mover al jugador si no hay colisión con las paredes
            transform.position = targetPosition;
        }
    }
    
    public void ChangeColor(Color color) => _rend.material.color = color;
}
