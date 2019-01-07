using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0.5f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    [HideInInspector] public bool playersTurn = true;

    private int level = 0;
    private bool doingSetup;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //add one to our level number 
        level++;

        //call init game to initialize our level
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;

        boardScript.SetupScene(level);

        doingSetup = false;
    }

    void OnEnable()
    {
        //tell our 'onlevelfinishedloading' function to start listening for a scene change event as soon as this script is enabled
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'onlevelfinishedloading' function to stop listening for a scene change event as soon as this script is disabled.
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    
    private void Update()
    {
        if (playersTurn || doingSetup)
            return;

        StartCoroutine(nextTurn());
    }

    IEnumerator nextTurn()
    {
        doingSetup = true;
        yield return new WaitForSeconds(turnDelay);

        yield return new WaitForSeconds(turnDelay);

        playersTurn = true;
        doingSetup = false;
    }
}
