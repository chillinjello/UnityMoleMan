using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 7;
    public int rows = 7;
    public GameObject player;
    public GameObject[] exit;
    public GameObject[] floorTiles;
    public GameObject[] rockTiles;
    public GameObject[] sunflowerTiles;
    public GameObject[] trapTiles;
    public GameObject[] squirrelTiles;
    public TextAsset[] levels;

    public int season;
    public int sunflowersRequired;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private enum ObjectTypes
    {
        EMPTY,
        ROCK,       //01
        HOLE,       //02
        SUNFLOWER,  //03
        TRAP,       //04
        SQUIRREL,   //05
        SNOWSHOES,  //06
        SPRINKLER_HEADS,  //07
        SMALL_SPRING_FLOWERS,   //08
        GROWN_SPRING_FLOWERS,   //09
        LEFT_GHOST, //10
        RIGHT_GHOST,//11
        UP_GHOST,   //12
        DOWN_GHOST, //13
        LEFT_PLANK, //14
        RIGHT_PLANK,//15
        UP_PLANK,   //16
        DOWN_PLANK, //17
        PUMPKIN,    //18
        PRESENT,    //19
        CHRISTMAS_SQUIRREL, //20
        GRAVE_SQUIRREL, //21
        GRAVE_SUNFLOWER, //22
        GRAVE_TRAP, //23
        WALL,
        NUMBER_OF_OBJECT_TYPES
    }
    private const int MOLE = 99;
    private const int MOLE_HOLE = 98;


    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        //create the board object to hold all the pieces
        boardHolder = new GameObject("Board").transform;

        //add the floor tiles to the game object
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                //create a new floor tile
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                //rotate the floor tile's sprite
                SpriteRenderer spriteRenderer = toInstantiate.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                    switch(Random.Range(0,3))
                    {
                        case 0:
                            spriteRenderer.flipX = true;
                            spriteRenderer.flipY = true;
                            break;
                        case 1:
                            spriteRenderer.flipX = false;
                            spriteRenderer.flipY = true;
                            break;
                        case 3:
                            spriteRenderer.flipX = true;
                            spriteRenderer.flipY = false;
                            break;
                        default:
                            spriteRenderer.flipX = false;
                            spriteRenderer.flipY = false;
                            break;
                    }
                }

                //add the tile to the board object
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.transform.SetParent(boardHolder);
            }
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        string[][] levelArray = getLevelData(level);
        LayoutObjects(levelArray);
    }

    void LayoutObjects(string [][] levelArray)
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                GameObject tileChoice;
                switch (Int32.Parse(levelArray[x][y]))
                {
                    case 0:
                        break;
                    case (int)ObjectTypes.ROCK:
                        tileChoice = rockTiles[Random.Range(0, rockTiles.Length)];
                        Instantiate(tileChoice, new Vector3(y,rows - x - 1,0f), Quaternion.identity);
                        break;
                    case (int)ObjectTypes.SUNFLOWER:
                        tileChoice = sunflowerTiles[Random.Range(0, sunflowerTiles.Length)];
                        Instantiate(tileChoice, new Vector3(y, rows - x - 1, 0f), Quaternion.identity);
                        break;
                    case (int)ObjectTypes.SQUIRREL:
                        tileChoice = squirrelTiles[Random.Range(0, squirrelTiles.Length)];
                        Instantiate(tileChoice, new Vector3(y, rows - x - 1, 0f), Quaternion.identity);
                        break;
                    case (int)ObjectTypes.TRAP:
                        tileChoice = trapTiles[Random.Range(0, trapTiles.Length)];
                        Instantiate(tileChoice, new Vector3(y, rows - x - 1, 0f), Quaternion.identity);
                        break;
                    case (int)ObjectTypes.HOLE:
                        tileChoice = exit[Random.Range(0, exit.Length)];
                        Instantiate(tileChoice, new Vector3(y, rows - x - 1, 0f), Quaternion.identity);
                        break;
                    case MOLE:
                        Instantiate(player, new Vector3(y, rows - x - 1, 0f), Quaternion.identity);
                        break;
                    case MOLE_HOLE:
                        tileChoice = exit[Random.Range(0, exit.Length)];
                        Instantiate(tileChoice, new Vector3(y, rows - x - 1, 0f), Quaternion.identity);
                        Instantiate(player, new Vector3(y, rows - x - 1, 0f), Quaternion.identity);
                        break;
                    default:
                        Debug.Log("Trying to add a invalid object type to the game");
                        break;
                }
            }
        }
    }

    string[][] getLevelData(int level)
    {
        //get the level string
        string levelText = getLevelString(level);

        //split up each row
        string[] lines = Regex.Split(levelText, "\r\n");

        //create the object you're going to return
        string[][] levelBase = new string[rows][];

        //break up the meta data
        string[] metaData = Regex.Split(lines[0], " ");

        //setup sunflower values
        sunflowersRequired = Int32.Parse(metaData[0]);

        //setup season value
        season = Int32.Parse(metaData[1]);

        //break up the rest of the rows
        for (int i = 1; i < lines.Length; i++)
        {
            string[] stringsOfLine = Regex.Split(lines[i], " ");
            levelBase[i - 1] = stringsOfLine;
        }

        //return the tiled levelBase
        return levelBase;
    }

    string getLevelString(int level)
    {
        TextAsset levelAsset;
        if (level <= levels.Length)
            levelAsset = levels[level - 1];
        else
            levelAsset = levels[0];

        return levelAsset.text;
    }
}
