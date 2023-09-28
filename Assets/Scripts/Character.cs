using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Node openDoor;
    [SerializeField] private float speed;
    [SerializeField] private float energy;
    [SerializeField] private Node currentNode;
    [SerializeField] private Node lastNode;

    private bool stopMove = false;

    public Node OpenDoor
    {
        get { return openDoor; }
        set { openDoor = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float Energy
    {
        get { return energy; }
        set { energy = value; }
    }

    public Node CurrentNode
    {
        get { return currentNode; }
        set { currentNode = value; }
    }

    public Node LastNode
    {
        get { return lastNode; }
        set { lastNode = value; }
    }

    public void SetPosition(Node node)
    {
        transform.position = node.transform.position;
        currentNode = node;
    }

    public void MoveChar()
    {
        Node nextNode;

        if (lastNode != null)
            nextNode = currentNode.GetRandomEdgeNodeWithCanMove(lastNode);
        else
            nextNode = currentNode.GetRandomEdgeNodeWithCanMove();

        if (nextNode != null)
        {
            MoveToNode(nextNode);
        }
    }

    public void MoveToNode(Node destinationNode)
    {
        Vector3 destinationPosition = destinationNode.transform.position;

        StartCoroutine(MoveCharacter(destinationPosition, destinationNode));
    }

    // Método para mover o personagem gradualmente até a posição de destino
    private IEnumerator MoveCharacter(Vector3 destination, Node destinationNode)
    {
        lastNode = currentNode;
        currentNode = destinationNode;

        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;

            if (stopMove)
            {
                stopMove = false;
                yield break;
            }
        }

        MoveChar();
    }


    public void MoveToPath(List<Node> path)
    {
        stopMove = true;
        StartCoroutine(MoveCharacter(path));
    }

    private IEnumerator MoveCharacter(List<Node> graphi)
    {
        foreach (Node node in graphi)
        {
            Vector3 destinationPosition = node.transform.position;

            lastNode = currentNode;
            currentNode = node;

            while (Vector3.Distance(transform.position, destinationPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinationPosition, speed * Time.deltaTime);
                yield return null;

                if (stopMove)
                {
                    stopMove = false;
                    yield break;
                }
            }
        }

        MoveChar();
    }
}
