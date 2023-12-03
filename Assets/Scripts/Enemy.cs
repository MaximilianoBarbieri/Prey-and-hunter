using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<Node> patrolNodes = new(); // Lista de nodos predeterminados
    private int _currentNodePatrol = 0;

    public float velocidadMovimiento = 2f;
    
    void Start()
    {
        //Posicionamos al enemy en el primer nodo de patrullaje
        transform.position = patrolNodes[_currentNodePatrol].transform.position;
    }

    private void FixedUpdate()
    {
        Patrullar();
    }

    void Patrullar()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolNodes[_currentNodePatrol].transform.position, velocidadMovimiento * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, patrolNodes[_currentNodePatrol].transform.position) < 0.1f)
        {
            // Avanzar al siguiente nodo de la lista
            _currentNodePatrol = (_currentNodePatrol + 1) % patrolNodes.Count;
        }
    }
}
