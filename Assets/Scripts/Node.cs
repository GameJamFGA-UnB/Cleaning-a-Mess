using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private bool canMove;
    [SerializeField] private bool isExit;
    [SerializeField] private List<Node> edges = new();

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    public List<Node> Edges
    {
        get { return edges; }
        set { edges = value; }
    }

    public void AddEdge(Node node)
    {
        edges.Add(node);
    }

    public Node GetRandomEdgeNodeWithCanMove()
    {
        // Crie uma lista temporária para armazenar os nós com CanMove = true
        List<Node> validEdges = new List<Node>();

        foreach (Node edge in edges)
        {
            if (edge.isExit)
                return edge;

            if (edge.CanMove)
            {
                validEdges.Add(edge);
            }
        }

        if (validEdges.Count == 0)
        {
            // Retorna null se nenhum nó com CanMove = true for encontrado
            return null;
        }
        else
        {
            // Gere um índice aleatório entre 0 e o número de nós válidos - 1
            int randomIndex = Random.Range(0, validEdges.Count);
            return validEdges[randomIndex];
        }
    }

    public Node GetRandomEdgeNodeWithCanMove(Node lastNode)
    {
        // Crie uma lista temporária para armazenar os nós com CanMove = true
        List<Node> validEdges = new List<Node>();

        foreach (Node edge in edges)
        {
            if (edge.isExit)
                return edge;

            if (edge.CanMove && edge != lastNode)
            {
                validEdges.Add(edge);
            }
        }

        if (validEdges.Count == 0)
        {
            // Retorna null se nenhum nó com CanMove = true for encontrado
            return GetRandomEdgeNodeWithCanMove();
        }
        else
        {
            // Gere um índice aleatório entre 0 e o número de nós válidos - 1
            int randomIndex = Random.Range(0, validEdges.Count);
            return validEdges[randomIndex];
        }
    }

    public bool isAdjacente(Node node)
    {
        return Edges.Contains(node) && node != this;
    }
}
