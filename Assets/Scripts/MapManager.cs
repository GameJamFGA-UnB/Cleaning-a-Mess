using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Node startNode;


    public Node StartNode
    {
        get { return startNode; }
        set { startNode = value; }
    }
}
