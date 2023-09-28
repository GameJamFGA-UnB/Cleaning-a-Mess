using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Node openDoor;
    [SerializeField] private float speed;
    [SerializeField] private float energy;
    [SerializeField] private Node currentNode;
    [SerializeField] private Node lastNode;

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
        // Verifique se o nó de destino é válido
        if (destinationNode == null)
        {
            Debug.LogError("Nó de destino inválido.");
            return;
        }

        // Calcula a trajetória até o nó de destino (pode ser uma trajetória simples como uma linha reta)
        Vector3 destinationPosition = destinationNode.transform.position;

        // Move o personagem para o nó de destino
        StartCoroutine(MoveCharacter(destinationPosition, destinationNode));
    }

    // Método para mover o personagem gradualmente até a posição de destino
    private IEnumerator MoveCharacter(Vector3 destination, Node destinationNode)
    {
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }

        // Quando o personagem chegar ao destino, atualize o currentNode para o novo nó
        currentNode = destinationNode;

        MoveChar();
    }
}
