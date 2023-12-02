using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    List<Node> _neighbors = new List<Node>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FindNeighbors()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position))
        {
            
        }
    }
}
