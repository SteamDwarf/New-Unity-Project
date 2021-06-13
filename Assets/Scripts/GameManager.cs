using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class GameManager : MonoBehaviour
{
    private DungeonGenerator dungeonGenerator;
    private GameObject pauseMenu;
    private AstarPath pathfinder;
    //public PlayerMovement player;
    public int aliveEnemies;
    public bool isPaused { get; set; }

    void Start() {
        dungeonGenerator = GetComponent<DungeonGenerator>();
 
        dungeonGenerator.InitializeDungeon();
        dungeonGenerator.GenerateDungeon();

        pathfinder = GameObject.Find("A*").GetComponent<AstarPath>();
        pathfinder.Scan();
    }

    void Update()
    {
        if(aliveEnemies <= 0)
        {
            SceneManager.LoadScene(3);
        }

        /*if (isPaused)
            pauseMenu.SetActive(true);
        else
            pauseMenu.SetActive(false);*/
    }

    public void ResumeGame()
    {
        isPaused = false;
    }

}
