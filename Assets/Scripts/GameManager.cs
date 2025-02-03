using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(this); }
    }

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject menuCanvas;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject resumeButton;

    private static bool hasStartedGame = false;
    private static bool gameIsPaused = false;

    public bool canPauseGame = true;

    private void Start()
    {
        Time.timeScale = 0f;
    }

    private void Update()
    {   // You can only pause the game if you clicked on "Start New Game" button
        if (Input.GetKeyDown(KeyCode.Escape) && hasStartedGame && canPauseGame)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
        player.GetComponent<PlayerMovement>().canTakeDamage = false;

        canvas.SetActive(false);
        //player.SetActive(false);

        menuCanvas.SetActive(true);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        player.GetComponent<PlayerMovement>().canTakeDamage = true;
        menuCanvas.SetActive(false);
        canvas.SetActive(true);
    }

    // When pressed it will restart the entire game and close the main menu
    public void StartNewGame()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        player.GetComponent<PlayerMovement>().canTakeDamage = true;
        hasStartedGame = true;

        menuCanvas.SetActive(false);
        resumeButton.SetActive(true);

        canvas.SetActive(true);
        player.SetActive(true);

        // Put code here to reset XP, upgrades, maxfloats, etc
    }
}
