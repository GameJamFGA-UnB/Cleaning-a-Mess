using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Algorithms
{
    public static List<Node> BFS(List<Node> nodes, Node s, Node t)
    {   // Complexidade operacao O(n+m)

        int[] visited = new int[nodes.Count];
        Array.Fill(visited, -1);

        Queue<Node> queue = new();
        List<Node> path = new();

        visited[s.Id] = s.Id;

        queue.Enqueue(s);

        while (queue.Any())
        {   // Complexidade operacao O(n), considerando que todos os nos serao emfileirados uma unica vez

            Node node = queue.Dequeue();

            foreach (Node nodeAdj in node.Edges)
            {   // Complexidade operacao O(m), O(deg(nodeAdj)) para cada vertice, no pior caso percorre todos os nos, sendo um somatorio dos graus de cada no igual a 2m

                if (!nodeAdj.CanMove)
                    continue;

                int id = nodeAdj.Id;

                if (visited[id] == -1)
                {
                    visited[id] = node.Id;
                    queue.Enqueue(nodeAdj);

                    if (nodeAdj.Equals(t))
                    {
                        Node temp = t;

                        while (!temp.Equals(s))
                        {   // O(n), pois pode no maximo passar por todos os nos para chegar ao destino

                            path.Insert(0, temp);
                            temp = nodes[visited[temp.Id]];

                        }
                        path.Insert(0, s);
                        return path;
                    }
                }
            }
        }

        return new List<Node>(); // Retorna uma lista vazia quando nao ha caminho entre s e t
    }

    public static List<Node> BFS(List<Node> nodes, Node s, Node t, bool isLunatic)
    {   // Complexidade operacao O(n+m)

        int[] visited = new int[nodes.Count];
        Array.Fill(visited, -1);

        Queue<Node> queue = new();
        List<Node> path = new();

        visited[s.Id] = s.Id;

        queue.Enqueue(s);

        while (queue.Any())
        {   // Complexidade operacao O(n), considerando que todos os nos serao emfileirados uma unica vez

            Node node = queue.Dequeue();

            foreach (Node nodeAdj in node.Edges)
            {   // Complexidade operacao O(m), O(deg(nodeAdj)) para cada vertice, no pior caso percorre todos os nos, sendo um somatorio dos graus de cada no igual a 2m

                int id = nodeAdj.Id;

                if (visited[id] == -1)
                {
                    visited[id] = node.Id;
                    queue.Enqueue(nodeAdj);

                    if (nodeAdj.Equals(t))
                    {
                        Node temp = t;

                        while (!temp.Equals(s))
                        {   // O(n), pois pode no maximo passar por todos os nos para chegar ao destino

                            path.Insert(0, temp);
                            temp = nodes[visited[temp.Id]];

                        }
                        path.Insert(0, s);
                        return path;
                    }
                }
            }
        }

        return new List<Node>(); // Retorna uma lista vazia quando nao ha caminho entre s e t
    }
}
