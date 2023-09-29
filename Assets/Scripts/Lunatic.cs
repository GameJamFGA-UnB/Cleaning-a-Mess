using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunatic : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float speed;
    [SerializeField] private Node currentNode;
    [SerializeField] private Node lastNode;
    [SerializeField] private Animations animations;
    [SerializeField] private float timeAnimationBroken;
    [SerializeField] private int hitNumber;


    public void StartMoveLunatic()
    {
        animations = GetComponent<Animations>();
        LoadNextNode();
    }

    public void SetPosition(Node node)
    {
        transform.position = node.transform.position;
        currentNode = node;
        lastNode = currentNode;
        animations.LoadNewMove();
        
    }

    public void LoadNextNode()
    {
        if (currentNode.Id == gameManager.Character.CurrentNode.Id)
        {
            return;
        }

        List<Node> nextNode = Algorithms.BFS(
            gameManager.GraphiManager.Nodes,
            currentNode,
            gameManager.Character.CurrentNode,
            true);

        if (nextNode.Count > 1)
            MoveToNode(nextNode[1]);
        else
            LoadNextNode();
    }

    public void MoveToNode(Node destinationNode)
    {
        Vector3 destinationPosition = destinationNode.transform.position;
        
        StartCoroutine(MoveCharacter(destinationPosition, destinationNode));
    }

    private IEnumerator MoveCharacter(Vector3 destination, Node destinationNode)
    {
        lastNode.IsLunatic = false;

        lastNode = currentNode;
        currentNode = destinationNode;

        currentNode.IsLunatic = true;

        animations.LoadNewMove(lastNode.transform.position, currentNode.transform.position);

        float min;

        if (destinationNode.CanMove)
            min = 0.1f;
        else
            min = 0.4f;

        while (Vector3.Distance(transform.position, destination) > min)
        {

            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;

        }

        if (destinationNode.CanMove)
            LoadNextNode();
        else
            StartCoroutine(BrokenDoor(destinationNode));
    }

    IEnumerator BrokenDoor(Node doorBroker)
    {
        for (int i = 0; i < hitNumber; i++)
        {
            animations.LoadNewMove();

            yield return new WaitForSeconds(timeAnimationBroken);
        }

        doorBroker.DisableDoor();
        LoadNextNode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Character>())
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
