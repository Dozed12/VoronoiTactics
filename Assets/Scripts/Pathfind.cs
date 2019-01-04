using System;
using System.Collections.Generic;

//Basic Dijkstra
//TODO At some point might be worth to implement A*

//Usage

/*

    Graph g = new Graph();
    g.vertex(1, new Dictionary<int, int>() {{2, 7}, {3, 8}});
    g.vertex(2, new Dictionary<int, int>() {{1, 7}, {6, 2}});
    g.vertex(3, new Dictionary<int, int>() {{1, 8}, {6, 6}, {7, 4}});
    g.vertex(4, new Dictionary<int, int>() {{6, 8}});
    g.vertex(5, new Dictionary<int, int>() {{8, 1}});
    g.vertex(6, new Dictionary<int, int>() {{2, 2}, {3, 6}, {4, 8}, {7, 9}, {8, 3}});
    g.vertex(7, new Dictionary<int, int>() {{3, 4}, {6, 9}});
    g.vertex(8, new Dictionary<int, int>() {{5, 1}, {6, 3}});

    g.path(1, 8).ForEach( x => Console.WriteLine(x) );
 */

class Graph
{
    Dictionary<int, Dictionary<int, int>> vertices = new Dictionary<int, Dictionary<int, int>>();

    public void vertex(int id, Dictionary<int, int> edges)
    {
        vertices[id] = edges;
    }

    public List<int> path(int start, int finish)
    {
        var previous = new Dictionary<int, int>();
        var distances = new Dictionary<int, int>();
        var nodes = new List<int>();

        List<int> path = null;

        foreach (var vertex in vertices)
        {
            if (vertex.Key == start)
            {
                distances[vertex.Key] = 0;
            }
            else
            {
                distances[vertex.Key] = int.MaxValue;
            }

            nodes.Add(vertex.Key);
        }

        while (nodes.Count != 0)
        {
            nodes.Sort((x, y) => distances[x] - distances[y]);

            var smallest = nodes[0];
            nodes.Remove(smallest);

            if (smallest == finish)
            {
                path = new List<int>();
                while (previous.ContainsKey(smallest))
                {
                    path.Add(smallest);
                    smallest = previous[smallest];
                }

                break;
            }

            if (distances[smallest] == int.MaxValue)
            {
                break;
            }

            foreach (var neighbor in vertices[smallest])
            {
                var alt = distances[smallest] + neighbor.Value;
                if (alt < distances[neighbor.Key])
                {
                    distances[neighbor.Key] = alt;
                    previous[neighbor.Key] = smallest;
                }
            }
        }

        return path;
    }
}

