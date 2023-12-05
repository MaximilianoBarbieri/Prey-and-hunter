using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    public WaitForSeconds time = new WaitForSeconds(0.025f);
    
    public List<Vector3> AStar(Node start, Node goal)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, int> nodeCost = new Dictionary<Node, int>();
        nodeCost.Add(start, 0);

        Node current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            current.NewColor(Color.grey);

            if (current == goal) break;

            foreach (var next in current.GetNeighbors())
            {
                //if (next.Blocked) continue; SE COMENTO YA QUE NO DEBERIA HABER BLOQUEOS

                int newCost = nodeCost[current] + next.Cost;

                if (!nodeCost.ContainsKey(next))
                {
                    nodeCost.Add(next, newCost);
                    frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                    cameFrom.Add(next, current);
                    next.NewColor(Color.red);
                }
                else if (newCost < nodeCost[current])
                {
                    frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                    nodeCost[next] = newCost;
                    cameFrom[next] = current;
                }

            }
        }

        List<Vector3> path = new List<Vector3>();
        if (current != goal) return path;

        while (current != start)
        {
            path.Add(current.transform.position);
            current.NewColor(Color.cyan);
            current = cameFrom[current];
        }

        return path;
    }

    float Heuristic (Vector3 start, Vector3 goal) => Vector3.Distance(start, goal);
    
    public IEnumerator AStarCoroutine(Node start, Node goal)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, int> nodeCost = new Dictionary<Node, int>();
        nodeCost.Add(start, 0);

        Node current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            current.NewColor(Color.grey);

            yield return time;

            if (current == goal)
            {
                while (current != start)
                {
                    current.NewColor(Color.cyan);
                    current = cameFrom[current];
                    yield return time;
                }
                break;
            }

            foreach (var next in current.GetNeighbors())
            {
                //if (next.Blocked) continue; SE COMENTO YA QUE NO DEBERIA HABER BLOQUEOS

                int newCost = nodeCost[current] + next.Cost;

                if (!nodeCost.ContainsKey(next))
                {
                    nodeCost.Add(next, newCost);
                    frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                    cameFrom.Add(next, current);
                    next.NewColor(Color.red);
                }
                else if (newCost < nodeCost[current])
                {
                    frontier.Enqueue(next, newCost + Heuristic(next.transform.position,goal.transform.position));
                    nodeCost[next] = newCost;
                    cameFrom[next] = current;
                    next.NewColor(new Color(0.7169812f, 0, 0.6866083f));
                }

            }
            yield return time;
        }
    }
}
