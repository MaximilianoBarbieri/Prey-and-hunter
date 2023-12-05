using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private List<Node> _neighbors = new();
    [SerializeField] LayerMask _nodeLayer;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] private float _findRadius;
    private Renderer _renderer;

    int _cost = 1;
    public int Cost => _cost;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        DetectNeighborNodes();
    }

    void DetectNeighborNodes()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _findRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Node") &&
                collider != this.GetComponent<Collider>() &&
                InFieldOfView(collider.transform.position))
                _neighbors.Add(collider.transform.GetComponent<Node>());
        }
    }

    bool InFieldOfView(Vector3 endPos)
    {
        Vector3 dir = endPos - transform.position;
        if (dir.magnitude > _findRadius) return false;
        if (!InLineOfSight(transform.position, endPos)) return false;
        return true;
    }

    bool InLineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, _wallLayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _findRadius);
    }
    
    public void NewColor(Color color)
    {
        _renderer.material.color = color;
    }

    public Color PreviousColor()
    {
        return _renderer.material.color;
    }
    public List<Node> GetNeighbors()
    {
        return _neighbors;
    }
}