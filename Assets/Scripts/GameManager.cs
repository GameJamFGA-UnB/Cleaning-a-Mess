using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<MapManager> mapManagers = new();
    [SerializeField] private int currentMap = 0;
    [SerializeField] private Character character;
    [SerializeField] private Node openedDoor;
    [SerializeField] private GraphiManager graphiManager;

    public void StartGame()
    {
        character.SetPosition(mapManagers[currentMap].StartNode);
        character.MoveChar();
    }

    public void OpenDoor(Node door)
    {
        if (openedDoor != null)
        {
            openedDoor.Action.ClickDoor();
            openedDoor = null;
        }

        openedDoor = door;
        
    }

    public Character Character 
    { 
        get { return character; } 
        set { character = value; } 
    }

    public GraphiManager GraphiManager
    {
        get { return graphiManager; }
        set { graphiManager = value; }
    }
}
