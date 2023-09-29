using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GraphiManager : MonoBehaviour
{
    [SerializeField] private GameObject edgePrefab;
    [SerializeField] private Transform edgeParent;
    [SerializeField] private List<Node> nodes;
    [SerializeField] private int[][] matrixAdj;
    [SerializeField] private float edgeHeight;
    [SerializeField] private bool createEdges;


    // Start is called before the first frame update
    void Start()
    {
        nodes ??= new List<Node>();

        int numNodes = nodes.Count;

        matrixAdj = new int[numNodes][];

        for (int i = 0; i < numNodes; i++)
        {
            matrixAdj[i] = new int[numNodes];
            nodes[i].Id = i;

            for (int j = 0; j < numNodes; j++)
            {
                matrixAdj[i][j] = 0;
            }
        }

        SetAdj();
    }

    public void SetAdj()
    {
        foreach (Node node in nodes)
        {
            if (node.IsDoor)
                node.CanMove = false;
            else
                node.CanMove = true;

            int currentNodeId = node.Id;

            foreach (Node adjNode in node.Edges)
            {
                int adjNodeId = adjNode.Id;

                matrixAdj[currentNodeId][adjNodeId] = 1;

                if (!adjNode.isAdjacente(node))
                {
                    adjNode.AddEdge(node);
                }

                if (matrixAdj[adjNodeId][currentNodeId] != 1)
                {
                    if (createEdges)
                        CreateEdge(node.transform.position, adjNode.transform.position);
                }
            }
        }
    }

    public void CreateEdge(Vector3 startNode, Vector3 endNode)
    {
        Vector3 position = (startNode + endNode) / 2f;

        float width = Vector3.Distance(startNode, endNode) * 5;
        float height = edgeHeight;

        GameObject edge = Instantiate(edgePrefab, position, Quaternion.identity);

        edge.transform.SetParent(edgeParent);

        edge.transform.localScale = new Vector3(width, height, 1f);

        Vector3 direction = (endNode - startNode).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        edge.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public List<Node> Nodes
    {
        get { return nodes; }
        set { nodes = value; }
    }
}
