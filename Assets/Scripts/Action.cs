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
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isOpen;


    private void Start()
    {
        if (IsLoop)
            StartCoroutine(StartAnimationLoop());
    }

    public void ForceCloseDoor(Action _action)
    {
        isOpen = false;
        StartCoroutine(CloseDoor(_action));
    }

    IEnumerator CloseDoor(Action _action)
    {
        yield return StartCoroutine(StartAnimationClose());

        gameManager.OpenedDoor = null;

        _action.ClickDoor();
    }

    public void ClickDoor()
    {
        if (StateManager.IsMoving) return;
        StateManager.IsMoving = true;

        Node openedDoor = gameManager.OpenedDoor;
        Node thisNode = GetComponent<Node>();

        if (openedDoor != null && thisNode != openedDoor)
        {
            openedDoor.CanMove = false;
            openedDoor.Action.ForceCloseDoor(this);
            return;
        }

        if (isOpen)
        {
            StartCoroutine(StartAnimationClose());
            isOpen = false;
            thisNode.CanMove = false;

            if (gameManager.Character.NextNode == thisNode)
            {
                gameManager.Character.StopMove = true;
                gameManager.Character.MoveCharAgain = true;
            }

            // verifico se o currentNode tem essa porta como No adjascente
            // se tiver eu chamo um funcao do char que verifica se o nextNode eh 
            // a porta, se for eu paro a movimentacao e volto para o currentNode,
            // depois eu continuo o Move()
        }
        else
        {
            StartCoroutine(StartAnimationOpen());
            isOpen = true;
            thisNode.CanMove = true;
            gameManager.OpenedDoor = thisNode;
            

            Node lastNode = gameManager.Character.LastNode;
            Node currentNode = gameManager.Character.CurrentNode;

            List<Node> graphi = gameManager.GraphiManager.Nodes;

            List<Node> pathLast = Algorithms.BFS(graphi, lastNode, thisNode);
            List<Node> pathCurrent = Algorithms.BFS(graphi, currentNode, thisNode);

            if (pathLast != null && pathCurrent != null)
            {
                List<Node> shortestPath = pathLast.Count <= pathCurrent.Count ? pathLast : pathCurrent;
                gameManager.Character.MoveToPath(shortestPath);
            }
            else
            {
                gameManager.Character.MoveChar();
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
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(timeAnimation);
            }
        }
    }

    IEnumerator StartAnimationClose()
    {
        gameManager.EffectsAudioCloseDoor.Play();
        for (int i = sprites.Count - 1; i >= 0; i--)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(timeAnimation);
        }

        StateManager.IsMoving = false;
    }

    IEnumerator StartAnimationOpen()
    {
        gameManager.EffectsAudioOpenDoor.Play();
        foreach (Sprite sprite in sprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(timeAnimation);
        }

        StateManager.IsMoving = false;
    }

    public SpriteRenderer SpriteRender
    {
        get { return spriteRenderer; }
        set { spriteRenderer = value; }
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
