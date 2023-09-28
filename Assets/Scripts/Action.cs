using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private float timeAnimation;
    [SerializeField] private bool isLoop;
    [SerializeField] private GameManager gameManager;

    private Image image;
    private bool isOpen;


    private void Start()
    {
        image = gameObject.GetComponent<Image>();

        if (IsLoop)
            StartCoroutine(StartAnimationLoop());
    }

    public void ClickDoor()
    {
        if (StateManager.IsMoving) return;
        StateManager.IsMoving = true;

        if (isOpen)
        {
            StartCoroutine(StartAnimationClose());
            isOpen = false;

            // verifico se o currentNode tem essa porta como No adjascente
            // se tiver eu chamo um funcao do char que verifica se o nextNode eh 
            // a porta, se for eu paro a movimentacao e volto para o currentNode,
            // depois eu continuo o Move()
        }
        else
        {
            StartCoroutine(StartAnimationOpen());
            isOpen = true;

            Node thisNode = GetComponent<Node>();

            Node lastNode = gameManager.Character.LastNode;
            Node currentNode = gameManager.Character.CurrentNode;

            List<Node> graphi = gameManager.GraphiManager.Nodes;

            List<Node> pathLast = Algorithms.BFS(graphi, lastNode, thisNode);
            List<Node> pathCurrent = Algorithms.BFS(graphi, currentNode, thisNode);

            if (pathLast != null && pathCurrent != null)
            {
                List<Node> shortestPath = pathLast.Count < pathCurrent.Count ? pathLast : pathCurrent;
                thisNode.CanMove = true;
                gameManager.Character.MoveToPath(shortestPath);
            }


            // BFS do lastNode e do currentNode para ver se eh possivel, se for para a personagem
            // manda ela ir ate o no que foi possivel ou o menor, e depois segue a bfs.
        }
    }

    IEnumerator StartAnimationLoop()
    { 
        while (true)
        {
            foreach (Sprite sprite in sprites)
            {
                image.sprite = sprite;
                yield return new WaitForSeconds(timeAnimation);
            }
        }
    }

    IEnumerator StartAnimationClose()
    {
        for (int i = sprites.Count - 1; i >= 0; i--)
        {
            image.sprite = sprites[i];
            yield return new WaitForSeconds(timeAnimation);
        }

        StateManager.IsMoving = false;
    }

    IEnumerator StartAnimationOpen()
    {
        foreach (Sprite sprite in sprites)
        {
            image.sprite = sprite;
            yield return new WaitForSeconds(timeAnimation);
        }

        StateManager.IsMoving = false;
    }

    public List<Sprite> Sprites
    {
        get { return sprites; }
        set { sprites = value; }
    }

    public float TimeAnimation
    {
        get { return timeAnimation; }
        set { timeAnimation = value; }
    }

    public bool IsLoop
    {
        get { return isLoop; }
        set { isLoop = value; }
    }

    public GameManager GameManager
    {
        get { return gameManager; }
        set { gameManager = value; }
    }
}
