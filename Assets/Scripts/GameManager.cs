using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    [SerializeField] private AudioSource trackAudioMenu;
    [SerializeField] private AudioSource trackAudioGame;
    [SerializeField] private AudioSource effectsAudioSlamming;
    [SerializeField] private AudioSource effectsAudioBroken;
    [SerializeField] private AudioSource effectsAudioOpenDoor;
    [SerializeField] private AudioSource effectsAudioCloseDoor;
    [SerializeField] private AudioSource effectsAudioFakeDoor;
    [SerializeField] private List<GameObject> prefabMaps;
    [SerializeField] private Transform parentMaps;
    [SerializeField] private GameObject currentMapObj;
    [SerializeField] private Vector3 positionMap;
    [SerializeField] private GameObject menuWin;
    [SerializeField] private GameObject menuLose;

    public void StartGame()
    {
        currentMap = Random.Range(0, prefabMaps.Count);

        trackAudioMenu.Stop();
        trackAudioGame.Play();

        currentMapObj = Instantiate(prefabMaps[currentMap], parentMaps);
        currentMapObj.transform.position = positionMap;
        graphiManager.Nodes = currentMapObj.GetComponent<MapManager>().Nodes;
        graphiManager.LoadNodes();
        
        GameObject _character = Instantiate(prefabChar, parentChars);
        _character.transform.position = spawPositionChar;
        character = _character.GetComponent<Character>();
        character.GameManager = this;

        GameObject _lunatic = Instantiate(prefabLunatic, parentChars);
        _lunatic.transform.position = spawPositionLunatic;
        lunatic = _lunatic.GetComponent<Lunatic>();
        lunatic.GameManager = this;

        character.SetPosition(currentMapObj.GetComponent<MapManager>().StartNode);
        character.MoveChar();

        StartCoroutine(StartLunaticMove());
    }

    public void Lose()
    {
        Destroy(character.gameObject);
        Destroy(lunatic.gameObject);
        Destroy(currentMapObj);

        trackAudioMenu.Play();
        trackAudioGame.Stop();

        OpenedDoor = null;

        menuLose.gameObject.SetActive(true);
    }

    public void Win()
    {
        Destroy(character.gameObject);
        Destroy(lunatic.gameObject);
        Destroy(currentMapObj);

        trackAudioMenu.Play();
        trackAudioGame.Stop();

        OpenedDoor = null;

        menuWin.gameObject.SetActive(true);
    }

    IEnumerator StartLunaticMove()
    {
        yield return new WaitForSeconds(5);
        lunatic.SetPosition(currentMapObj.GetComponent<MapManager>().StartNode);
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

    public AudioSource TrackAudioMenu
    {
        get { return trackAudioMenu; }
        set { trackAudioMenu = value; }
    }

    public AudioSource TrackAudioGame
    {
        get { return trackAudioGame; }
        set { trackAudioGame = value; }
    }

    public AudioSource EffectsAudioBroken
    {
        get { return effectsAudioBroken; }
        set { effectsAudioBroken = value; }
    }

    public AudioSource EffectsAudioOpenDoor
    {
        get { return effectsAudioOpenDoor; }
        set { effectsAudioOpenDoor = value; }
    }

    public AudioSource EffectsAudioCloseDoor
    {
        get { return effectsAudioCloseDoor; }
        set { effectsAudioCloseDoor = value; }
    }

    public AudioSource EffectsAudioSlamming
    {
        get { return effectsAudioSlamming; }
        set { effectsAudioSlamming = value; }
    }

    public AudioSource EffectsAudioFakeDoor
    {
        get { return effectsAudioFakeDoor; }
        set { effectsAudioFakeDoor = value;}
    }
}
