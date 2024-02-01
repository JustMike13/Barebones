using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    CharacterManager player;
    [SerializeField]
    CharacterManager opponent;
    [SerializeField]
    OverlayMenu overlay;

    // Start is called before the first frame update
    void Start()
    {
        overlay.ChangeState(OverlayMenu.START);
        player.LevelManager = this;
        player.IsPlayer = true;
        opponent.LevelManager = this;
    }

    public void Defeat()
    {
        DisableControllers();
        overlay.ChangeState(OverlayMenu.GAMEOVER);
    }

    public void Victory()
    {
        DisableControllers();
        overlay.ChangeState(OverlayMenu.VICTORY);
    }

    private void DisableControllers()
    {
        player.Disable();
        opponent.Disable();
    }
}
