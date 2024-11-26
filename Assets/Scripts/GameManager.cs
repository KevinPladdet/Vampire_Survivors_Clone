using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject player;

    private void Start()
    {
        Time.timeScale = 0f;
    }

    public void PlayGame()
    {
        menuCanvas.SetActive(false);
        canvas.SetActive(true);
        player.SetActive(true);
    }
}
