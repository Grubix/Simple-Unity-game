using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class maze : MonoBehaviour
{

    public GameObject player;

    public GameObject wall;

    public GameObject wallHalf;

    public GameObject highWall;

    public GameObject trap;

    private static char WALL = '█';
    private static char WALL_HALF = '▀';
    private static char TRAP = 'x';
    private static char SPAWN = '*';

    void Start()
    {
        string maze = Resources.Load<TextAsset>("mazes/maze1").text;
        string[] mazeLines = maze.Split('\n');
        Array.Reverse(mazeLines);

        for(int z=0; z<mazeLines.Length; z++) {
            string line = mazeLines[z]; 

            for(int x=0; x<line.Length; x++) {
                if(line[x] == WALL) {
                    if(z == 0 || z == mazeLines.Length-1 || x == 0 || x == line.Length - 2) {
                        GameObject.Instantiate(highWall, new Vector3(x * 1.5f, 4.5f, z * 1.5f), Quaternion.identity);
                    } else {
                        GameObject.Instantiate(wall, new Vector3(x * 1.5f, 1.5f, z * 1.5f), Quaternion.identity);
                    }
                } else if(line[x] == WALL_HALF) {
                    if(z == 0 || z == mazeLines.Length-1 || x == 0 || x == line.Length - 2) {
                        GameObject.Instantiate(highWall, new Vector3(x * 1.5f, 0.005f, z * 1.5f), Quaternion.identity);
                    } else {
                        GameObject.Instantiate(wallHalf, new Vector3(x * 1.5f, 2.25f, z * 1.5f), Quaternion.identity);
                    }
                } else if(line[x] == TRAP) {
                    GameObject.Instantiate(trap, new Vector3(x * 1.5f, 0.005f, z * 1.5f), Quaternion.identity);
                } else if(line[x] == SPAWN) {
                    player.transform.position = new Vector3(x * 1.5f + 0.75f, 10f, z * 1.5f + 0.75f);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
