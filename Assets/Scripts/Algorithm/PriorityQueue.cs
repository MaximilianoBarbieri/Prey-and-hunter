using System.Collections.Generic;

public class PriorityQueue <T>
{
    Dictionary<T, float> _allElements = new Dictionary<T, float>();
    public int Count { get { return _allElements.Count; } }

    public void Enqueue(T elem,float cost)
    {
        if (!_allElements.ContainsKey(elem)) _allElements.Add(elem, cost);
        else _allElements[elem] = cost;
    }

    public T Dequeue()
    {
        if(_allElements.Count == 0) return default;
        T elem = default;

        foreach (var item in _allElements)
        {
            if(elem == null)
            {
                elem = item.Key;
                continue;
            }
            if (item.Value < _allElements[elem]) elem = item.Key;
        }
        _allElements.Remove(elem);
        return elem;
    }
}
