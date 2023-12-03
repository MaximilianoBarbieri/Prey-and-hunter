using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] public Transform[] patrolNodes; // Lista de nodos predeterminados
    private int startNodePatrol = 0;

    public float velocidadMovimiento = 5f;
    public float distanciaDeteccion = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        //Posicionamos al enemy en el primer nodo de patrullaje
        transform.position = patrolNodes[startNodePatrol].position;
    }

    // Update is called once per frame
    void Update()
    {
        Patrullar();
    }
    
    void Patrullar()
    {
        
      // Obtener el nodo actual
        Transform currentNode = patrolNodes[startNodePatrol];

          /*// Moverse hacia el nodo actual
        transform.position = Vector3.MoveTowards(transform.position, nodoActual.position, velocidadMovimiento * Time.deltaTime);*/

        // Comprobar si ha llegado al nodo actual
        if (Vector3.Distance(transform.position, currentNode.position) < 0.3f)
        {
            // Cambiar al siguiente nodo en la lista de patrulla
            startNodePatrol = (startNodePatrol + 1) % patrolNodes.Length;
        }

        // Realizar raycasts en las cuatro direcciones (Norte, Sur, Este, Oeste)
        RealizarRaycast(Vector3.up); // Norte
        RealizarRaycast(Vector3.down); // Sur
        RealizarRaycast(Vector3.right); // Este
        RealizarRaycast(Vector3.left); // Oeste
    }
    
    void RealizarRaycast(Vector3 direccion)
    {
        // Lanzar un raycast en la dirección especificada
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direccion, out hit, distanciaDeteccion, LayerMask.GetMask("Node")))
        {
            // Si el raycast choca con un nodo, actualizar el nodo actual
            startNodePatrol = System.Array.IndexOf(patrolNodes, hit.transform);
        }

        // Dibujar el raycast en la escena para depuración
        Debug.DrawRay(transform.position, direccion * distanciaDeteccion, Color.red);
    }
}
