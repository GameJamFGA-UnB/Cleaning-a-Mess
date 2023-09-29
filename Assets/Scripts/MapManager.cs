using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Node startNode;
    [SerializeField] private List<Node> nodes;


    public Node StartNode
    {
        get { return startNode; }
        set { startNode = value; }
    }

    public List<Node> Nodes
    {
        get { return nodes; }
        set { nodes = value; }
    }
}
