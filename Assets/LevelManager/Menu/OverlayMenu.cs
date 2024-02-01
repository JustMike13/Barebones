using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayMenu : MonoBehaviour
{
    // constants
    public const int START = 0;
    public const int INGAME = 1;
    public const int PAUSE = 2;
    public const int VICTORY = 3;
    public const int GAMEOVER = 4;
    [SerializeField]
    Canvas startMenu;
    [SerializeField]
    Canvas inGameUI;
    [SerializeField]
    Canvas pauseMenu;
    [SerializeField]
    Canvas gameOverMenu;
    [SerializeField]
    Canvas victoryMenu;
    [SerializeField]
    ThirdPersonController player;
    [SerializeField]
    OpponentController opponent;
    int state = -1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == INGAME)
            {
                ChangeState(PAUSE);
            }
            else if (state == PAUSE)
            {
                ChangeState(INGAME);
            }
        }
    }

    public void ChangeState(int nextState)
    {
        if (state == nextState)
        {
            return;
        }
        startMenu.enabled = nextState == START;
        inGameUI.enabled = nextState == INGAME; 
        pauseMenu.enabled = nextState == PAUSE;
        gameOverMenu.enabled = nextState == GAMEOVER;
        victoryMenu.enabled = nextState == VICTORY;
        state = nextState;
        PauseGame(state == INGAME);
    }

    void PauseGame(bool onoff)
    {
        Time.timeScale = onoff ? 1 : 0;
        Cursor.visible = !onoff;
        Cursor.lockState = onoff ? CursorLockMode.Locked : CursorLockMode.None;
        player.LockRotation = !onoff;
    }

    public bool IsGameON()
    {
        return state == INGAME;
    }
}
