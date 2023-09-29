using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [Header("_______________________________________________________________________________________")]
    [Header("PROPRIEDADES")]
    [Space]
    [SerializeField] private int id;
    [SerializeField] private List<Node> edges = new();
    [SerializeField] private bool isLunatic;

    [Space]
    [Header("_______________________________________________________________________________________")]
    [Header("PORTA")]
    [Space]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool isDoor;
    [SerializeField] private bool canMove;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private float timeAnimation;
    [SerializeField] private bool isExit;
    [SerializeField] private bool isFakeDoor;

    private Action action;


    private void Start()
    {
        if (isDoor)
        {
            action = gameObject.GetComponent<Action>();

            if (action == null)
            {
                action = gameObject.AddComponent<Action>();
                action.Sprites = sprites;
                action.TimeAnimation = timeAnimation;
                action.GameManager = gameManager;
            }

            try
            {
                action.SpriteRender = transform.GetChild(0).GetComponent<SpriteRenderer>();
            }
            catch
            {
                Debug.LogError("Esqueceu da porta: " + gameObject.name);
            }

            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    private void OnMouseDown()
    {
        if (isDoor && action != null)
        {
            if (!isFakeDoor)
                action.ClickDoor();
            else
                gameManager.EffectsAudioFakeDoor.Play();
        }
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

            if (edge.CanMove && edge != lastNode && !edge.IsLunatic)
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

    public void DisableDoor()
    {
        canMove = true;
        isDoor = false;
        GetComponent<BoxCollider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        gameManager.EffectsAudioBroken.Play();
    }

    public bool isAdjacente(Node node)
    {
        return Edges.Contains(node) && node != this;
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public GameManager GameManager
    {
        get { return gameManager; }
        set { gameManager = value; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    public bool IsDoor
    {
        get { return isDoor; }
        set { isDoor = value; }
    }

    public List<Node> Edges
    {
        get { return edges; }
        set { edges = value; }
    }

    public Action Action
    {
        get { return action; }
        set { action = value; }
    }

    public bool IsLunatic
    {
        get { return isLunatic; }
        set { isLunatic = value; }
    }

    public bool IsExit
    {
        get { return isExit; }
        set { isExit = value; }
    }
}
