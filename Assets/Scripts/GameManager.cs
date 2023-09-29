using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<MapManager> mapManagers = new();
    [SerializeField] private int currentMap = 0;
    [SerializeField] private Transform parentChars;
    [SerializeField] private GameObject prefabChar;
    [SerializeField] private Character character;
    [SerializeField] private GameObject prefabLunatic;
    [SerializeField] private Lunatic lunatic;
    [SerializeField] private Node openedDoor;
    [SerializeField] private GraphiManager graphiManager;
    [SerializeField] private Vector3 spawPositionChar;
    [SerializeField] private Vector3 spawPositionLunatic;

    public void StartGame()
    {
        GameObject _character = Instantiate(prefabChar, parentChars);
        _character.transform.position = spawPositionChar;
        character = _character.GetComponent<Character>();

        GameObject _lunatic = Instantiate(prefabLunatic, parentChars);
        _lunatic.transform.position = spawPositionLunatic;
        lunatic = _lunatic.GetComponent<Lunatic>();
        lunatic.GameManager = this;

        character.SetPosition(mapManagers[currentMap].StartNode);
        character.MoveChar();

        StartCoroutine(StartLunaticMove());
    }

    IEnumerator StartLunaticMove()
    {
        yield return new WaitForSeconds(5);
        lunatic.SetPosition(mapManagers[currentMap].StartNode);
        lunatic.StartMoveLunatic();
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

    public Node OpenedDoor
    {
        get { return openedDoor; }
        set { openedDoor = value; }
    }
}
