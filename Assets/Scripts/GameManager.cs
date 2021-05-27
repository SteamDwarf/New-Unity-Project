using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    DungeonGenerator dungeonGenerator;
    private GameObject pauseMenu;
    //public PlayerMovement player;
    public int aliveEnemies;
    public bool isPaused = false;

    void Start() {
        dungeonGenerator = GetComponent<DungeonGenerator>();

        dungeonGenerator.InitializeDungeon();
        dungeonGenerator.GenerateDungeon();

        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
    }

    void Update()
    {
        if(aliveEnemies <= 0)
        {
            SceneManager.LoadScene(3);
        }

        if (isPaused)
            pauseMenu.SetActive(true);
        else
            pauseMenu.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isPaused)
                ResumeGame();
            else
                isPaused = true;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
    }
}
