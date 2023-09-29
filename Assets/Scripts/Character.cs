using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Node currentNode;
    [SerializeField] private Node lastNode;
    [SerializeField] private Node nextNode;
    [SerializeField] private Animations animations;

    private bool stopMove = false;
    private bool moveCharAgain = false;
    [SerializeField] private bool isMoving = false;

    public void SetPosition(Node node)
    {
        transform.position = node.transform.position;
        currentNode = node;
        lastNode = node;
        animations.LoadNewMove();
    }

    public void MoveChar()
    {
        if (lastNode != null)
            nextNode = currentNode.GetRandomEdgeNodeWithCanMove(lastNode);
        else
            nextNode = currentNode.GetRandomEdgeNodeWithCanMove();

        if (nextNode != null)
        {
            MoveToNode(nextNode);
        }
        else
        {
            nextNode = null;
            isMoving = false;
            animations.LoadNewMove();
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

        animations.LoadNewMove(lastNode.transform.position, currentNode.transform.position);
        isMoving = true;

        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;

            if (stopMove)
            {
                stopMove = false;
                isMoving = false;
                if (moveCharAgain)
                {
                    moveCharAgain = false;
                    MoveChar();
                }
                yield break;
            }
        }

        MoveChar();
    }


    public void MoveToPath(List<Node> path)
    {
        //Debug.Log("Estou chamando a funcao para Mover para o Path");
        
        if (isMoving)
            stopMove = true;

        StartCoroutine(WaitToMove(path));
    }

    IEnumerator WaitToMove(List<Node> path)
    {
        //Debug.Log("Entrei em WaitToMove");

        while (stopMove == true)
        {
            yield return null;
        }

        StartCoroutine(MoveCharacter(path));
    }

    private IEnumerator MoveCharacter(List<Node> graphi)
    {
        //Debug.Log("Entrei em MoveCharacter(List<Node> graphi)");
        isMoving = true;
        foreach (Node node in graphi)
        {
            Vector3 destinationPosition = node.transform.position;

            lastNode = currentNode;
            currentNode = node;

            animations.LoadNewMove(lastNode.transform.position, currentNode.transform.position);

            while (Vector3.Distance(transform.position, destinationPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinationPosition, speed * Time.deltaTime);
                yield return null;

                if (stopMove)
                {
                    stopMove = false;
                    isMoving = false;
                    if (moveCharAgain)
                    {
                        moveCharAgain = false;
                        MoveChar();
                    }
                    yield break;
                }
            }
        }

        MoveChar();
    }

    public Node NextNode
    {
        get { return currentNode; }
        set { currentNode = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
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

    public bool StopMove
    {
        get { return stopMove; }
        set { stopMove = value; }
    }

    public bool MoveCharAgain
    {
        get { return moveCharAgain; }
        set { moveCharAgain = value; }
    }
}
