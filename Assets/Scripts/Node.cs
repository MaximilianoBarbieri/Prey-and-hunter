using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private List<Node> _neighbors = new();
    [SerializeField] LayerMask _wallLayer;
    int _cost = 1;
    public int Cost => _cost;

    // Start is called before the first frame update
    void Start()
    {
        RealizarRaycasts();
    }

    // Update is called once per frame
    void Update()
    {
    }


    void RealizarRaycasts()
    {
        // Direcciones en orden de las agujas del reloj (Norte, Este, Sur, Oeste)
        Vector3[] direcciones = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

        foreach (Vector3 direccion in direcciones)
        {
            // Lanzar un raycast en la dirección especificada
            RaycastHit hit;

            // Configurar la capa de máscara para incluir "Wall" y "Node"
            int layerMask = LayerMask.GetMask("Wall", "Node");

            // Realizar el raycast con la capa de máscara configurada
            if (Physics.Raycast(transform.position, direccion, out hit, 5, layerMask))
            {
                Debug.DrawRay(transform.position, direccion * hit.distance, Color.red, 100f);

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall")) continue;
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Node"))
                    _neighbors.Add(hit.transform.GetComponent<Node>());
            }
        }
    }

    bool IsValidNeighbourd(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, _wallLayer);
    }

    public List<Node> GetNeighbors()
    {
        return _neighbors;
    }
}